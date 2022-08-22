# Csv generator with dynamic fields

## Introduction 

Project was designed for quick creation of Csv streams with records contain some dynamic fields

## How to use

1. Create a model implements ICustomFields interface (Will require to add CustomFields to model)

![Alt text](/model_example.png?raw=true "Optional Title")

2. Instantiate model with Dynamic fields. Dynamic fields are Dictionary where key is header name and value is actual value
Create a list of dynamic fields you want to include. 

![Alt text](/model_instantiation.png?raw=true "Optional Title")

Important: Only headers specified in this and met to keys from previous step will be generated

3. Call csv generation

![Alt text](/call_csv_generator.png?raw=true "Optional Title")

4. See results

![Alt text](/generated_csv_file.png?raw=true "Optional Title")




