# Falcon Pi Twitter

This project is designed for Falcon Pi Player to provide updates via Twitter on the light show that
you are running. Those updates include posting the current song and providing alerts when problems
are detected.

This application is designed to run on Falcon Pi Players that are installed on Raspberry Pi.

For more information about this project, visit the
<a href="https://thealmostengineer.com/projects/falcon-pi-twitter" target="_blank">project page</a>.

## Example

To see what the tweets will look like, visit the
<a href="https://twitter.com/hplightshow" target='_blank'>HP Light Show page</a>.

## Installation

* Go to the Plugins Manager in your FPP setup.
* Paste the following URL in the box at the top of the page:
[https://raw.githubusercontent.com/almostengr/falconpitwitter/release/plugininfo.json](https://raw.githubusercontent.com/almostengr/falconpitwitter/release/plugininfo.json)
* Click "Get Plugin Info"
* The plugin will apppear underneath the text box. Click "Install"
* After installation has completed, go to File Manager > Uploads. You will see a file named
falconpitwitter.json.  Download this file to your computer.
* Open this falconpitwitter.json in the text editor of your choice.
* Create a [Twitter](https://twitter.com) account.
* Sign up for the [Twitter Developer Platform](https://developer.twitter.com) with the TWitter account that
you just created. Once logged in and approved, you will need to create a new project and generate the
Consumer Key, Consumer Secret, Access Token, and Access Secrets. Paste each of values in the corresponding
field in the falconpitwitter.json file.
* Update the remaining configuration values. See the Configuration section below for more details.
* Upload the edited file to your Falcon Pi Player.
* Restart FPPD
* Play a song from one of your playlists. If things are working correctly, you will see the song information
as a tweet. If things are NOT working correctly, go to the File Manager > Logs and check the system log
for errors.

## Configuration

### Logging

To change the logging done by the application, you may lower or raise the logging level. By default,
only Information or higher severity messages are logged. We suggest that Debug logging not be turned
on unless you are experiencing a recurring problem.

```json
"LogLevel": {
    "Default": "Information",
    "Microsoft": "Warning",
    "Microsoft.Hosting.Lifetime": "Information"
}
```

### FppHosts

List each of the Falcon Player instances that you want to be monitored. The first instance is considered
to the primary instance. This instance should be set to ```master``` or ```standalone``` within
the Falcon Player settings. Each additional instance will be treated as a remote instance.
When no hosts are listed in this section, this will default to "http://localhost".

```json
"FppHosts": [
    "http://localhost/"
],
```

### Twitter Credentials

This section holds the values to access the Twitter API. Visit the
[Twitter for Developers](https://developer.twitter.com) page to sign up and get the needed keys and tokens.
You will need to get a Consumer Key (aka API Key), Consumer Secret (aka API Secret), Access Token and Access Secret
for this section.

```json
"Twitter": {
    "ConsumerKey": "8W4tZQ6xp7",
    "ConsumerSecret": "qJz6nDw2T7",
    "AccessToken": "KBiEB6jn28",
    "AccessSecret": "8nftJzHOAI",
},
```

When no value is provided for any of the properties, the application will experience issues and
stop running. Errors will show up in the application log.


### Monitoring

```json
"Monitoring": {
    "AlarmUserNames": [
        "@twitteruser"
    ],
    "MaxAlarmsPerHour": 3,
    "MaxCpuTemperatureC": 62.0
},
```

```AlarmUsernames``` should be the name of the Twitter account(s) that can be mentioned if
there is an issue with the show (e.g. Raspberry Pi having high CPU temperature). Value needs to include
the at (@) symbol. Each Twitter handle should be listed as a separate item in this file.
When no value has been provided, then alerts will show up as public tweets instead of mentions.
```MaxCpuTemperatureC``` should be the threshold that has to be reached before a high temperature alert is triggered.
In warmer climates, you will want to set this value higher to prevent false alerts.
This value needs to be in degrees Celsius. Per the Raspberry Pi documentation, 60 to 65
degrees Celsius is close to the safe upper operating limit of the Pi.
When no value has been provided, this will default to 60.0 degrees.
```MaxAlarmsPerHour``` is the number alarms that you will be notified about within an hour. Once this threshold
has been reached, you will not be notified again until the next hour. The alarms will still be reported
in the application log. To receive infinite alerts, set this value to ```0```.
When no value has been provided, this will default to 3 alerts per hour.

### Example appsettings.json File

Once you have finished updating the appsettings.json file, it should look similar to the example below.
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
        "FppHosts": [
            "http://localhost/"
        ],
        "Monitoring": {
            "AlarmUserNames": [
                "@twitteruser"
            ],
            "MaxAlarmsPerHour": 3,
            "MaxCpuTemperatureC": 62.0
        },
        "Twitter": {
            "ConsumerKey": "8W4tZQ6xp7",
            "ConsumerSecret": "qJz6nDw2T7",
            "AccessToken": "KBiEB6jn28",
            "AccessSecret": "8nftJzHOAI",
        }
    }
}
```


## Troubleshooting

### System Service Output / Log

To see the logged output from the system service, login to FPP via SSH and run the command:

```sh
journalctl -u fptworker -b
```

### Additional Questions and Answers

Visit the <a href="https://thealmostengineer.com/projects/falcon-pi-twitter" target="_blank">project page</a>
for more information to common questions and answers.
