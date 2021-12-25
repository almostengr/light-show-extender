#!/bin/bash

####################################################################################
# DESCRIPTION: Remove Falcon Pi Twitter from the system
# AUTHOR: Kenny Robinson, @almostengr
# CREATED: 2021-06-20
####################################################################################

cd /home/fpp/media/plugins/

echo "Stopping the system service"

sudo systemctl stop falconpitwitter

echo "Disabling the system service"

sudo systemctl disable falconpitwitter

sudo systemctl daemon-reload

echo "Removing the application service"

sudo rm -f /lib/systemd/system/falconpitwitter.service

echo "Removing the application files"

sudo rm -rf /home/fpp/media/plugins/falconpitwitter
