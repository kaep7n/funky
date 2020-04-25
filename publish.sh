#!/bin/bash

if [ "$1" == "" ]; then
    echo 'token must not be empty'
    return 1
fi

echo 'creating github source'
dotnet nuget add source https://nuget.pkg.github.com/kaep7n/index.json -n github -u kaep7n -p $1 --store-password-in-clear-text

echo 'pushing nuget packages'
dotnet nuget push **/*.nupkg -s github