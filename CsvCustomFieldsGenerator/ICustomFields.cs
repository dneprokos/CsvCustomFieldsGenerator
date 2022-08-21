using System.Collections.Generic;

namespace CsvCustomFieldsGenerator
{
    public interface ICustomFields
    {
        Dictionary<string, string> CustomFields { get; set; }
    }
}
