using System.Collections.Generic;

namespace CsvCustomFieldsGenerator.Tests.TestModels
{
    public class CountryTestCsvModel : ICustomFields
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string CapitalName { get; set; }
        public int Population { get; set; }
        public Dictionary<string, string> CustomFields { get; set; }
    }
}
