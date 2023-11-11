# System Configuration

System configuration is maintained in the appsettings.json file for the application. 

## Sample JSON File

```json
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft": "Warning",
            "Microsoft.Hosting.Lifetime": "Information"
        }
    },
    "AppSettings": {
        "NwsApiUrl": "",
        "NwsStationId": "KMGM",
        "FalconSetting": {
            "ApiUrl": "",
            "MaxCpuTemperatureC": 62.0
        },
        "FrontEnd": {
            "ApiUrl": "",
            "ApiKey": ""
        }
    }
}
```

To configure the application, copy the appsettings.template.json file to appsettings.json in the same 
directory as the application. 
All values are required to be populated. Values that are missing, will be populated with default values by
the application. Those default values, may result in requests not completing.
