# Falcon Pi Monitor

This project provides a way for the Falcon Pi Player to post the current playing 
song and alerts on Twitter.

## Technology

This project uses C# and TweetInvi.

## Installation Instructions

* Download the latest release that is available in zip or tar format.
* Copy the archive file to your Raspberry Pi.
* Extract the archive file contents. Ideally extract them to a folder in the /home/fpp directory.
* Create a [Twitter Developer account](https://developer.twitter.com/). 
* Once approved, create a project. 
Within that project, create Consumer Key (aka API Key), Consumer Secret (aka API Secret), Access Token and Access Secret.
Also within that project, update the App Permissions to "Read and Write". By default, permissions are "Read".
* Copy appsettings.template.json to appsettings.json.
* Add the key, secrets, and token that you got from your Twitter developer account to the appsettings.json file.
See [Example appsettings.json File](#example-appsettings.json-file) and 
[About appsettings.json File](#about-appsettings.json-file) for explainations and details.
* Create a cronjob that will run the automation on startup. See [Creating Cronjob](#cronjob) for explaination.
* Reboot your Raspberry Pi
* Once the Pi has come back online, check the log file to confirm that the monitor has started. 
You should see output similar to the below at the beginning of the log file.
```
12/10/2020 11:05:52 PM | Monitoring show
12/10/2020 11:05:52 PM | Exit program by pressing Ctrl+C
12/10/2020 11:06:11 PM | Connected to Twitter as hpchristmas
```

The "Connected to Twitter" message in the log file, confirms that your account has been properly configured
and can post to Twitter.

### Creating Cronjob

Create a cronjob that runs on reboot. On your FPP, open a SSH session. Once logged in, enter

```sh
crontab -e
```

When the text editor opens, add the following to the bottom of the file. Change the directory to match 
where you extracted the FP Monitor.

```
@reboot /home/fpp/fpmonitor/falconpimonitor > /home/fpp/media/logs/falconpimonitor.log 2>&1
```

Then save and exit the text editor.

### Example appsettings.json File

Once you have added the Twitter key, token, and secrets to the appsettings.json file, it should look like 
the following: 

```json
{
    "TwitterSettings": {
        "ConsumerKey": "8W4tZQ6xp7",
        "ConsumerSecret": "qJz6nDw2T7",
        "AccessToken": "KBiEB6jn28",
        "AccessSecret": "8nftJzHOAI",
    },
    "AlarmSettings": {
        "AlarmUser": "@XrGOEz2Wc7",
        "Temperature": 55.0
    },
    "FppShow":{
        "PostOffline": false
    },
    "FalconPiSettings":{
        "FalconUri": "http://falconpi/"
    }
}
```

### About appsettings.json File

* "AlarmUser" should be the name of the Twitter account that can be mentioned if 
there is an issue with the show (e.g. Raspberry Pi having high CPU temperature). Value needs to include 
the at (@) symbol.
* "FalconUri" should be the hostname or IP address to your Falcon Pi Player. If your FPP does not have an 
assigned or static IP address, then I would recommend using the hostname.
* "Temperature" should be the threshold that has to be reached before a high temperature alert is triggered.
In warmer climates, you will want to set this value higher to prevent false alerts.
Per the Raspberry Pi documentation, 60 to 65 s
degrees Celsius is close to the upper operating limit of the Pi. This value needs to be in degrees Celsius

## Known Bugs

### Exception on First Run

An exception will occur in the log if the Wifi connection has not been established before the first run. Confirm
in the log that HttpRequestException is not repeating in the logs after 2 or 3 attempts.

## Questions / Comments

Please file an Issue on the repo if you have questions, comments, or bug reports about this application.
