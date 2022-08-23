# Csv generator with dynamic fields

## Introduction 

Project was designed for quick creation of Csv streams with records contain some dynamic fields

## How to use

1. Create a model implements ICustomFields interface (Will require to add CustomFields to model)

```

    public class CountryTestCsvModel : ICustomFields
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string CapitalName { get; set; }
        public int Population { get; set; }
        public Dictionary<string, string> CustomFields { get; set; }
    }

```

2. Instantiate model with Dynamic fields. Dynamic fields are Dictionary where key is header name and value is actual value

```
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
```


3. Create a list of dynamic fields you want to include. 

```

    var headers = new List<string>
    {
        "Language: Ukrainian", "Language: English"
    };

```

Important: Only headers specified in this and met to keys from previous step will be generated

4. Instantiate helper and call one of the csv generation methods

```

    var csvGenerator = new CsvStreamGenerator();
    var stream = csvGenerator.GenerateCsvStream(headers, csvRows, false);

```

5. Next you can save your stream to some csv file. As an example, here is how to save it locally

```
    var filePath = Directory.GetCurrentDirectory();
    const string fileName = "Test.csv";
    SaveStreamAsFile(filePath, stream, fileName);

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

```

6. See results

```

| CountryId | Name    | CapitalName | Population | Language: Ukrainian | Language: English |
| 1	        | Ukraine |	Kyiv        | 41167336   | Primary             | Secondary         |


```





