#!/bin/bash

####################################################################################
# DESCRIPTION: Remove Falcon Pi Twitter from the system
# AUTHOR: Kenny Robinson, @almostengr
# CREATED: 2021-06-20
####################################################################################

cd /home/fpp/media/plugins/

sudo systemctl disable falconpitwitter

sudo systemctl stop falconpitwitter

sudo systemctl daemon-reload

sudo rm -rf /home/fpp/media/plugins/falconpitwitter

sudo rm -f /lib/systemd/system/falconpitwitter.service
