# Falcon Pi Twitter

This project is designed for Falcon Pi Player to provide updates via Twitter on the light show that
you are running. Those updates include posting the current song and providing alerts when problems
are detected.

This application is designed to run on Falcon Pi Players that are installed on Raspberry Pi.

For more information about this project, visit the
[project page](https://thealmostengineer.com/projects/falcon-pi-twitter).

## Table of Contents

* [Example](#example)
* [Installation](#installation)
* [Configuration](#configuration)
* [Frequently Asked Questions](#faqs-frequently-asked-questions)

## Example

To see what the tweets will look like, visit the
[HP Light Show Twitter page](https://twitter.com/hplightshow).

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
* Sign up for the [Twitter Developer Platform](https://developer.twitter.com) with the Twitter account that
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

## FAQs (Frequently Asked Questions)

### System Service Output / Log

To see the logged output from the system service, login to FPP via SSH and run the command: 

```sh
journalctl -u falconpitwitter -b
```

If any errors occur with the application, they wil show up in the system service log.

### Tweeting Song Information

This application calls the Falcon Pi Player API to get the meta data for the song that is current playing. 
Then it uses that information to compose a tweet. If the song that is playing does not have ID3 tag 
information entered, then will not display part or all of the song data. If you need to add the song 
meta data to the file, you can use a program like 
[Audacity](https://www.audacityteam.org) to do so.

### Tweeting Alarms (or Alerts)

The application calls the Falcon Pi Player API to get the current temperature of the Raspberry Pi. 
If it is above the threshold that is specified in the [appsettings.json](#configuration)
file, then it will send a tweet
that mentions the users specified in the [appsettings.json](#configuration)
file a message to let them know if the 
current temperature.

### How frequently are checks done? 

Songs are checked every 15 seconds to see if it has changed. If the same song is playing from the
previous check, then no tweet is posted. 

Vitals are checked every 15 minutes. Alarms are based on the settings that you have defined in the
[configuration file](#configuration).

### I don't want certain playlists to post song information. How do I accomplish this? 

Any playlist that has "offline" or "testing" (case insensitive) in the name of it, will not post 
the song information to 
Twitter. The vitals alarms can still be triggered when "offline" or "testing" playlists are active.

### Where is the source code?

Source code for this project is hosted on 
[Github](https://github.com/almostengr/falconpitwitter). The latest release
can also be downloaded from here.

### "Are you connected to internet? HttpRequest Exception occured" shows in the log. What does this mean? 

This means that your Falcon Pi Player instance attempted to connect to the internet or another device but 
was not able to do so. Double check your network and internet connection to ensure that data can be sent.
Also double check your configuration file as the hostname(s) may be incorrect or mistyped.

### Why did you build a standalone application instead of an FPP plugin?

I work as a software developer primarily building web-based applications in C#. 
Based on what I have seen, most (if not all) of the FPP plugins are build with PHP. While I do know PHP and
have worked with it in the past, I chose to go with building a C# application. 
as it gave me an opportunity to use my existing skills and 
expand them by applying them to something different than what I am used to.

### Additional Questions and Answers

Visit the [project page](https://thealmostengineer.com/projects/falcon-pi-twitter)
for more information to common questions and answers.
