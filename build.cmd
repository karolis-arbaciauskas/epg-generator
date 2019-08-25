dotnet restore src
dotnet lambda package --project-location src --configuration release --framework netcoreapp2.1 --output-package bin/release/netcoreapp2.1/epg.zip
