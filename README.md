# Falcon Pi Monitor

This project is designed for Falcon Pi Player to provide updates via Twitter on the light show that 
you are running. Those updates include posting the current song and providing alerts when problems
are detected.

This application is ONLY designed to run on Falcon Pi Players that are installed on Raspberry Pi.

## More Information

For more information, including installation instructions, how to set up the service cronjob, and 
how to configure the appsettings.json, visit the 
<a href="http://lightshow.thealmostengineer.com/other-information/falconpimonitor/" 
target="_blank">project page</a> on the HP Light Show website.

## Twitter Examples

Follow my Christmas Light Show account [@hpchristmas](https://twitter.com/hpchristmas) to see what this 
application can do.

## Known Bugs

### Exception on First Run

An exception will occur in the log if the Wifi connection has not been established before the first run. Confirm
in the log that HttpRequestException is not repeating in the logs after 2 or 3 attempts.

### Duplicate Log Entries

Log entries are duplicated after project refactoring. Issue #11 has been opened to track the work on 
this effort.

## Questions / Comments

Please file an Issue on the repo if you have questions, comments, or bug reports about this application.

You can also reach out to the developer via Twitter [@almostengr](https://twitter.com/almostengr).