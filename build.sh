#!/bin/bash

echo 'building version' $1

if [ "$1" != "" ]; then
    echo 'replacing current version with ' $1
    # replace all version properties with given version
   find . -type f \( -iname "*.csproj" ! -iname "*.Tests.csproj" \) -exec sed -i "s/<Version>.*<\/Version>/<Version>$1<\/Version>/" {} +
fi

# build solution
dotnet build src/Funky.sln --configuration Release
# test solution
dotnet test src/Funky.sln --configuration Release