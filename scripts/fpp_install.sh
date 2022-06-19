#!/bin/bash

####################################################################################
# DESCRIPTION: Commit files and update tags after main branch has been updated.
# AUTHOR: Kenny Robinson, @almostengr
# CREATED: 2021-06-20
####################################################################################

cd /home/fpp/media/uploads

echo "Downloading application"

/bin/wget https://github.com/almostengr/falconpitwitter/releases/latest/download/falconpitwitter.tar.gz -O falconpitwitter.tar.gz

echo "Unpacking application files to project directory"

/bin/mkdir -p /home/fpp/media/plugins/falconpitwitter

/bin/tar -xf /home/fpp/media/uploads/falconpitwitter.tar.gz --directory /home/fpp/media/plugins/falconpitwitter

/bin/cp -p /home/fpp/media/plugins/falconpitwitter/appsettings.template.json /home/fpp/media/uploads/falconpitwitter.json

echo "Installing service"

sudo /bin/cp /home/fpp/media/plugins/falconpitwitter/falconpitwitter.service /lib/systemd/system

sudo /bin/systemctl daemon-reload

sudo /bin/systemctl enable falconpitwitter

echo "Installation complete."

echo "First time users, you will need to enter your configuration settings in the Uploads directory." 
echo "After updating the configuration, you will need to restart the service"
echo "or reboot the Pi."
echo ""
echo "Upgrading users, reboot your device to make sure the new application loads."
echo ""
echo "See https://thealmostengineer.com/projects/falcon-pi-twitter for more information."
echo ""
