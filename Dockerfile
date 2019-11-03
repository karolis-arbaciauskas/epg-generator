FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY src/TV24Generator/*.csproj ./TV24Generator/
RUN dotnet restore

# copy everything else and build app
COPY TV24Generator/. ./TV24Generator/
WORKDIR /app/TV24Generator
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=build /app/TV24Generator/out ./
ENTRYPOINT ["dotnet", "TV24Generator.dll"]