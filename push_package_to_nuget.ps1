param ([Parameter(Mandatory)]$PackageName, [Parameter(Mandatory)]$NuGetApiKey)
$source = "https://api.nuget.org/v3/index.json";
$packageLocation = "CsvCustomFieldsGenerator\bin\Debug";

if(-not($PackageName)) { 
    Write-Error You must specify PackageName e.g Csv.CustomFieldsGenerator.1.0.1.nupkg 
}
if(-not($NuGetApiKey)) { 
    Write-Error You must specify NuGetApiKey. You may generate it on a NuGet 
}
else {
    Write-Host "Publishing package "$PackageName" to NuGet source "$source""
    dotnet nuget push $packageLocation\$PackageName --api-key $NuGetApiKey --source $source
}
