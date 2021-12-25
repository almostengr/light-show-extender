# Falcon Pi Twitter

This project is designed for Falcon Pi Player to provide updates via Twitter on the light show that 
you are running. Those updates include posting the current song and providing alerts when problems
are detected.

This application is designed to run on Falcon Pi Players that are installed on Raspberry Pi.

## Problem

I wanted a way for viewers of my Christmas light show to be able to find out the song information 
about the show. In addition, I wanted to have an online presence for the show. While having a FM 
transmitter that can do RDS (Radio Data System) would have accomplished this task, I wanted a more technical 
way to have this task be able to be completed.

## Solution

I created this Falcon Pi Twitter application that will monitor the Falcon Pi Player that runs my 
Christmas light show. This monitor gets the status and other information based on the Falcon Pi Player
via the Falcon Pi Player API. Then the information is sent out as a tweet via the Light Show's Twitter account.

The application also monitors the Falcon Pi Player for abnomalities. Currently the application monitors the 
CPU temperature of the Raspberry Pi and whether a song gets stuck playing or the FPP Daemon freezes while 
playing a song.

For more information about this project, visit the 
<a href="https://thealmostengineer.com/projects/falcon-pi-twitter" target="_blank">project page</a>.

## Example

To see what the tweets will look like, visit the 
<a href="https://twitter.com/hplightshow" target='_blank'>HP Light Show page</a>.
