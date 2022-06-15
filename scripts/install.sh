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
sudo /bin/systemctl daemon-reload
sudo /bin/systemctl enable falconpitwitter

echo "Installation complete."

echo "First time users, you will need to enter your configuration settings." 
echo "This can be done by copying appsettings.template.json to appsettings.json"
echo "and entering your configuration values in appsettings.json."
echo "After updating the configuration, you will need to restart the service"
echo "or reboot the Pi."
echo ""
echo "Upgrading users, reboot your device to make sure the new application loads."
echo ""
echo "See https://thealmostengineer.com/projects/falcon-pi-twitter for more information."
echo ""
