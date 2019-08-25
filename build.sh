#!/bin/bash

#install zip on debian OS, since microsoft/dotnet container doesn't have zip by default
if [ -f /etc/debian_version ]
then
  apt -qq update
  apt -qq -y install zip
fi

dotnet restore src/aws-csharp.csproj
dotnet lambda package --project-location src --configuration release --framework netcoreapp2.1 --output-package bin/release/netcoreapp2.1/epg.zip
