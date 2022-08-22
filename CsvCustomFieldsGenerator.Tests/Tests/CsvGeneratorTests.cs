using System.Collections.Generic;
using System.IO;
using CsvCustomFieldsGenerator.Tests.TestModels;
using NUnit.Framework;

namespace CsvCustomFieldsGenerator.Tests.Tests
{

    public class CsvGeneratorTests
    {
        [Test]
        public void GenerateCsv_WithStaticAndCustomFields_OneRow_ShouldBeGenerated()
        {
            //Arrange
            var csvRows = new List<CountryTestCsvModel>
            {
                new  CountryTestCsvModel
                {
                    CountryId = 1, 
                    Name = "Ukraine",
                    CapitalName = "Kyiv",
                    Population = 41167336,
                    CustomFields = new Dictionary<string, string>
                    {
                        { "Language: Ukrainian", "Primary" },
                        { "Language: English", "Secondary" }
                    }
                }
            };

            var headers = new List<string>
            {
                "Language: Ukrainian", "Language: English"
            };

            //Act
            var csvGenerator = new CsvStreamGenerator();
            var stream = csvGenerator.GenerateCsvStream(headers, csvRows, false);

            //Assert
            var filePath = Directory.GetCurrentDirectory();
            const string fileName = "Test.csv";
            SaveStreamAsFile(filePath, stream, fileName);
        }

        private static void SaveStreamAsFile(string filePath, Stream inputStream, string fileName)
        {
            DirectoryInfo info = new DirectoryInfo(filePath);
            if (!info.Exists)
            {
                info.Create();
            }

            string path = Path.Combine(filePath, fileName);
            using (FileStream outputFileStream = new FileStream(path, FileMode.Create))
            {
                inputStream.CopyTo(outputFileStream);
            }
        }
    }
}
