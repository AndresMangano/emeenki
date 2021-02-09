FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /hermes-api
COPY . .
WORKDIR /hermes-api/API
RUN dotnet publish -c Release

FROM mcr.microsoft.com/dotnet/sdk:5.0
WORKDIR /hermes-api
COPY --from=build /hermes-api/API/bin/Release/net5.0/publish .
EXPOSE 8080
ENTRYPOINT [ "dotnet", "API.dll", "--urls", "http://0.0.0.0:8080" ]