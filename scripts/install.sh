#!/bin/bash

####################################################################################
# DESCRIPTION: Commit files and update tags after main branch has been updated.
# AUTHOR: Kenny Robinson, @almostengr
# CREATED: 2021-06-20
####################################################################################

cd /home/fpp/media/uploads

echo "Downloading application"

wget https://github.com/almostengr/falconpitwitter/releases/latest/download/falconpitwitter.tar.gz -O falconpitwitter.tar.gz

echo "Unpacking application files to project directory"

mkdir -p /home/fpp/media/plugins

gunzip /home/fpp/media/uploads/falconpitwitter.tar.gz

mkdir -p /home/fpp/media/plugins/falconpitwitter

tar -xf /home/fpp/media/uploads/falconpitwitter.tar --directory /home/fpp/media/plugins/falconpitwitter

echo "Installing service"

sudo cp /home/fpp/media/plugins/falconpitwitter/falconpitwitter.service /lib/systemd/system
sudo systemctl daemon-reload
sudo systemctl enable falconpitwitter

echo "Installation complete."

echo "First time users, update the configuration file located at "
echo "/home/fpp/media/plugins/falconpitwitter/appsettings.json"
echo "and restart the service or reboot the Pi."
echo ""
echo "Upgrading users, reboot your device to make sure the new application loads."
echo ""
echo "See https://thealmostengineer.com/projects/falcon-pi-twitter for more information."
echo ""