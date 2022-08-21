using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;

namespace CsvCustomFieldsGenerator
{
    public class CsvStreamGenerator
    {
        /// <summary>
        /// Generates CSV steam with dynamic fields
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dynamicHeaders">dynamic header names e.g. CustomerHeader: Dynamic header</param>
        /// <param name="records">Records you want to generate</param>
        /// <param name="withoutHeaders">If you don't want first line to be a header</param>
        /// <returns></returns>
        public Stream GenerateCsvStream<T>(List<string> dynamicHeaders, List<T> records, bool withoutHeaders) 
            where T : ICustomFields
        {
            VerifyRecordsIsNotNullOrEmpty(records);

            //------------CSV Stream Creation------------
            var firstRecord = records.First();
            var properties = firstRecord.GetType().GetProperties();

            var stringWriter = new StringWriter();
            var csv = new CsvWriter(stringWriter, CultureInfo.InvariantCulture);

            if (!withoutHeaders)
            {
                //Write Headers
                foreach (var propertyInfo in properties)
                {
                    //NOTE: Write custom headers
                    if (propertyInfo.Name == nameof(ICustomFields.CustomFields))
                    {
                        foreach (var header in dynamicHeaders)
                        {
                            csv.WriteField(header);
                        }
                    }
                    else //NOTE: Write static headers
                    {
                        csv.WriteField(propertyInfo.Name);
                    }
                }
                csv.NextRecord();
            }

            //Write rows
            records.ForEach(record =>
            {
                var innerProperties = record.GetType().GetProperties();

                foreach (var property in innerProperties)
                {
                    if (dynamicHeaders.Count > 0 && property.Name == nameof(firstRecord.CustomFields))
                    {
                        //TODO: GET each key value pair and write value. Please note can be empty
                        var value = property.GetValue(record);

                        if (value == null)
                        {
                            dynamicHeaders.ForEach(header => csv.WriteField(string.Empty));
                        }
                        else
                        {
                            var keyValuePairs = value as Dictionary<string, string>;
                            // ReSharper disable once PossibleNullReferenceException
                            if (keyValuePairs.Count == dynamicHeaders.Count)
                            {
                                foreach (var keyValuePair in keyValuePairs)
                                {
                                    csv.WriteField(keyValuePair.Value);
                                }
                            }
                            else
                            {
                                foreach (var expectedHeader in dynamicHeaders)
                                {
                                    csv.WriteField(keyValuePairs.ContainsKey(expectedHeader)
                                        ? keyValuePairs[expectedHeader]
                                        : string.Empty);
                                }
                            }
                        }
                    }
                    else
                    {
                        var value = property.GetValue(record) ?? string.Empty;
                        csv.WriteField(value);
                    }
                }
                csv.NextRecord();
            });

            byte[] byteArray = Encoding.ASCII.GetBytes(stringWriter.ToString());
            var stream = new MemoryStream(byteArray);
            Console.WriteLine("CSV stream was generated");

            return stream;
        }

        /// <summary>
        /// Generates CSV steam with dynamic fields
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dynamicHeaders">dynamic header names e.g. CustomerHeader: Dynamic header</param>
        /// <param name="records">Records you want to generate</param>
        /// <param name="excludedHeaderValues">Model Headers and values you want to exclude from CSV</param>
        /// <returns></returns>
        public Stream GenerateCsvStream<T>(List<string> dynamicHeaders, List<T> records, List<string> excludedHeaderValues)
            where T : ICustomFields
        {
            VerifyRecordsIsNotNullOrEmpty(records);

            //------------CSV Stream Creation------------
            var firstRecord = records.First();
            var properties = firstRecord.GetType().GetProperties();

            var stringWriter = new StringWriter();
            var csv = new CsvWriter(stringWriter, CultureInfo.InvariantCulture);

            //Write Headers
            foreach (var propertyInfo in properties)
            {
                if (excludedHeaderValues.All(h => h != propertyInfo.Name))
                {
                    //NOTE: Write custom headers
                    if (propertyInfo.Name == nameof(ICustomFields.CustomFields))
                    {
                        foreach (var header in dynamicHeaders)
                        {
                            csv.WriteField(header);
                        }
                    }
                    else //NOTE: Write static headers
                    {
                        csv.WriteField(propertyInfo.Name);
                    }
                }
            }
            csv.NextRecord();

            //Write rows
            records.ForEach(customer =>
            {
                var innerProperties = customer.GetType().GetProperties();

                foreach (var property in innerProperties)
                {
                    if (excludedHeaderValues.All(h => h != property.Name))
                    {
                        if (dynamicHeaders.Count > 0 && property.Name == nameof(firstRecord.CustomFields))
                        {
                            //TODO: GET each key value pair and write value. Please note can be empty
                            var value = property.GetValue(customer);

                            if (value == null)
                            {
                                dynamicHeaders.ForEach(header => csv.WriteField(string.Empty));
                            }
                            else
                            {
                                var keyValuePairs = value as Dictionary<string, string>;
                                // ReSharper disable once PossibleNullReferenceException
                                if (keyValuePairs.Count == dynamicHeaders.Count)
                                {
                                    foreach (var keyValuePair in keyValuePairs)
                                    {
                                        csv.WriteField(keyValuePair.Value);
                                    }
                                }
                                else
                                {
                                    foreach (var expectedHeader in dynamicHeaders)
                                    {
                                        csv.WriteField(keyValuePairs.ContainsKey(expectedHeader)
                                            ? keyValuePairs[expectedHeader]
                                            : string.Empty);
                                    }
                                }
                            }
                        }
                        else
                        {
                            var value = property.GetValue(customer) ?? string.Empty;
                            csv.WriteField(value);
                        }
                    }
                }
                csv.NextRecord();
            });

            byte[] byteArray = Encoding.ASCII.GetBytes(stringWriter.ToString());
            var stream = new MemoryStream(byteArray);

            return stream;
        }

        #region Private helpers

        private void VerifyRecordsIsNotNullOrEmpty<T>(List<T> records)
        {
            if (records == null || records.Count == 0)
            {
                throw new Exception("Records should not be null or empty");
            }
        }

        #endregion
    }
}
