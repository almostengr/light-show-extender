#!/bin/bash

####################################################################################
# DESCRIPTION: Remove Falcon Pi Twitter from the system
# AUTHOR: Kenny Robinson, @almostengr
# CREATED: 2021-06-20
####################################################################################

cd /home/fpp/media/plugins/

echo "Stopping the system service"

sudo /bin/systemctl stop fptworker

echo "Disabling the system service"

sudo /bin/systemctl disable fptworker

sudo /bin/systemctl daemon-reload

echo "Removing the application service"

sudo /bin/rm -f /lib/systemd/system/fptworker.service

echo "Removing the application files"

sudo /bin/rm -rf /home/fpp/media/uploads/falconpitwitter.json
