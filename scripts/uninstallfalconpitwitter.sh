#!/bin/bash

####################################################################################
# DESCRIPTION: Remove Falcon Pi Twitter from the system
# AUTHOR: Kenny Robinson, @almostengr
# CREATED: 2021-06-20
####################################################################################

cd /home/fpp/

# remove service

sudo systemctl disable falconpitwitter

## stop service

sudo systemctl stop falconpitwitter

# reload

sudo systemctl daemon-reload

## remove files

rm -rf /home/fpp/media/falconpitwitter

rm -f /lib/systemd/system/falconpitwitter.service