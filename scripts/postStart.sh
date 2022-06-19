#!/bin/bash

PROCESSES=$(/bin/ps -ef | /bin/grep "Almostengr.FalconPiTwitter.Worker" | /bin/wc -l)

if (${PROCESSES} -ge 2)
    exit
fi

sudo /bin/systemctl restart fptworker.service
