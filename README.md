# Light Show Extender

## Problem

I wanted to have a way that visitors to the light show would be able to interact with the light show. While researching what others were doing, I found a way for this to be done.

## Solution

I found a project called Remote Falcon that allows visitors to do some of the similar things that I wanted. However, it did lack some features. Remote Falcon is maintained by one of the individuals in the light show community. It is a personal project of his and he does not charge fees for the service. I debated with whether to fork the existing repository and make changes to the existing code, but it was apprehensive about doing so.

What I decided to do was to create my own jukebox implementation of the light show using my website. My implementation of this project, was designed to have only the features that I desired to be included. This mainly consisted of users being able pick one of the songs that that they wanted to hear and the Falcon Pi Player, play those songs in the order in which they were requested.

## More Information

For more information about this project, visit the User Guide folder or
[project page](https://thealmostengineer.com/projects/light-show-extender).

## References and Guides

Additional information and coding examples that were used 

* https://www.parksq.co.uk/dotnet-core/dependency-injection-httpclient-and-ihttpclientfactory




## System Service Configuration

Below are the commands and expected output when setting up the system service for the Light Show
Extender application. By using a system service, this will ensure that the application always starts when the
device is powered on and that if the appllication unexpectedly crashes, that it is restarted automatically.

To set up the system service, you will need to copy the service file to the "systemd" directory. Then run
the rest of the commands listed.

```sh
fpp@fppyard:~/lightshowextender $ cat lightshowextender.service
[Unit]
Description=Light Show Extender
After=network.target
Documentation=https://thealmostengineer.com/projects/light-show-extender

[Service]
Type=simple
Restart=always
ExecStart=/home/fpp/lightshowextender/Almostengr.LightShowExtender.Worker
User=fpp

[Install]
WantedBy=multi-user.target

fpp@fppyard:~/lightshowextender $ sudo cp lightshowextender.service /lib/systemd/system
fpp@fppyard:~/lightshowextender $ sudo systemctl daemon-reload
fpp@fppyard:~/lightshowextender $ sudo systemctl enable lightshowextender
Created symlink /etc/systemd/system/multi-user.target.wants/lightshowextender.service → /lib/systemd/system/lightshowextender.service.
fpp@fppyard:~/lightshowextender $ sudo systemctl status lightshowextender
● lightshowextender.service - Light Show Extender
     Loaded: loaded (/lib/systemd/system/lightshowextender.service; enabled; vendor preset: enabled)
     Active: inactive (dead)
       Docs: https://thealmostengineer.com/projects/light-show-extender
fpp@fppyard:~/lightshowextender $ sudo systemctl start lightshowextender
fpp@fppyard:~/lightshowextender $ sudo systemctl status lightshowextender
● lightshowextender.service - Light Show Extender
     Loaded: loaded (/lib/systemd/system/lightshowextender.service; enabled; vendor preset: enabled)
     Active: active (running) since Sun 2023-09-10 16:16:33 CDT; 3s ago
       Docs: https://thealmostengineer.com/projects/light-show-extender
   Main PID: 23055 (Almostengr.Ligh)
      Tasks: 17 (limit: 1934)
        CPU: 4.067s
     CGroup: /system.slice/lightshowextender.service
             └─23055 /home/fpp/lightshowextender/Almostengr.LightShowExtender.Worker

Sep 10 16:16:33 fppyard systemd[1]: Started Light Show Extender.
Sep 10 16:16:33 fppyard Almostengr.LightShowExtender.Worker[23055]: Almostengr.LightShowExtender.Worker, Version=2023.9.10.0, Culture=neutral, PublicKeyToken=null
Sep 10 16:16:35 fppyard Almostengr.LightShowExtender.Worker[23055]: Microsoft.Hosting.Lifetime[0] Application started. Hosting environment: Production; Content root path: /
fpp@fppyard:~/lightshowextender $ journalctl -u lightshowextender -b
-- Journal begins at Fri 2023-06-30 06:44:53 CDT, ends at Sun 2023-09-10 16:17:04 CDT. --
Sep 10 16:16:33 fppyard systemd[1]: Started Light Show Extender.
Sep 10 16:16:33 fppyard Almostengr.LightShowExtender.Worker[23055]: Almostengr.LightShowExtender.Worker, Version=2023.9.10.0, Culture=neutral, PublicKeyToken=null
Sep 10 16:16:35 fppyard Almostengr.LightShowExtender.Worker[23055]: Microsoft.Hosting.Lifetime[0] Application started. Hosting environment: Production; Content root path: /
```



## System Configuration

System configuration is maintained in the appsettings.json file for the application. 

### Sample JSON File

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
