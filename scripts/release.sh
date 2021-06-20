#!/bin/bash

####################################################################################
# DESCRIPTION: Commit files and update tags after main branch has been updated.
# AUTHOR: Kenny Robinson, @almostengr
# CREATED: 2021-06-20
####################################################################################

STATUSOUTPUT=$(git status)

if [[ ${STATUSOUTPUT} != *"nothing to commit, working tree clean"* ]]; then
    echo "Updates were found"

    # git pull
    git config user.name github-actions
    git config user.email github-actions@github.com
    git add fptrelease
    git commit -m "Committed latest release"
    # git push

    LASTTAG=$(git tag | tail -1)
    CURRENTYEAR=$(date +%Y)

    if [[ "${LASTTAG}" == *"${CURRENTYEAR}"* ]]; then
        NEWTAG=$((${LASTTAG} + 1))
    else
        NEWTAG="${CURRENTYEAR}01"
    fi

    git tag ${NEWTAG}

    git push --tags

    git push

else
    echo "No updates were found"
    exit 0
fi
