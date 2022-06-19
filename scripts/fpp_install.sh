#!/bin/bash

####################################################################################
# DESCRIPTION: Commit files and update tags after main branch has been updated.
# AUTHOR: Kenny Robinson, @almostengr
# CREATED: 2021-06-20
####################################################################################

echo "Unpacking application files to project directory"

/bin/mkdir -p /home/fpp/media/plugins/falconpitwitter

/bin/cp -p /home/fpp/media/plugins/falconpitwitter/appsettings.template.json /home/fpp/media/upload/falconpitwitter.json

echo "Installing service"

sudo /bin/cp /home/fpp/media/plugins/falconpitwitter/scripts/fptworker.service /lib/systemd/system

sudo /bin/systemctl daemon-reload

sudo /bin/systemctl enable fptworker

echo "Installation complete."

echo "First time users, you will need to enter your configuration settings in the Uploads directory." 
echo "After updating the configuration, you will need to restart the service"
echo "or reboot the Pi."
echo ""
echo "Upgrading users, reboot your device to make sure the new application loads."
echo ""
echo "See https://thealmostengineer.com/projects/falcon-pi-twitter for more information."
echo ""
