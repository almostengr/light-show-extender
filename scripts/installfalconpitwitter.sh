#!/bin/bash

####################################################################################
# DESCRIPTION: Commit files and update tags after main branch has been updated.
# AUTHOR: Kenny Robinson, @almostengr
# CREATED: 2021-06-20
####################################################################################

cd /home/fpp/

## download files

wget https://raw.githubusercontent.com/almostengr/almostengrwebsite/main/fptrelease/falconpitwitter.tar -O falconpitwitter.tar

# create directory if it does not already exist

mkdir -p /home/fpp/media/falconpitwitter

## extract files from repo

tar -xf falconpitwitter.tar --directory /home/fpp/media/falconpitwitter

## install service

sudo cp /home/fpp/media/falconpitwitter/falconpitwitter.service /lib/systemd/system
sudo systemctl daemon-reload
sudo systemctl enable falconpitwitter

## start service

sudo systemctl start falconpitwitter