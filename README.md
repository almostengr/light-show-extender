# Falcon Pi Monitor

This project contains a set of actions that I use to monitor the status of my Falcon Pi Player. Those actions
are using to post updates via Twitter and alert if there are problems detected.

## Technology

This project uses C# and TweetInvi.

## Installation

* Download the latest release that is available.

### Cronjob

Create a cronjob that runs on reboot. On your FPP, open an SSH terminal window. THen enter

```sh
crontab -e
```

When the text editor opens, add the following to the bottom of the file.

```
@reboot /home/fpp/falconpimonitor/falconpimonitor > /home/fpp/falconpimonitor.log 2>&1
```

Then save and exit the text editor. After a couple of minutes, you should see falconpimonitor.log appear
in the /home/fpp directory. Check this file to confirm that there are no repeating errors in the log.

## Known Bugs

### Exception on first run

An exception will occur in the log if the Wifi connection has not been established before the first run. Confirm
in the log that HttpRequestException is not repeating in the logs after 2 or 3 attempts.

## Questions / Comments

Please file an Issue on the repo if you have questions, comments, bug reports about this application.