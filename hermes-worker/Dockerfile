FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /hermes-worker
COPY . .
RUN dotnet publish -c Release

FROM mcr.microsoft.com/dotnet/runtime:5.0
WORKDIR /hermes-worker
COPY --from=build /hermes-worker/bin/Release/net5.0/publish .
EXPOSE 8000
ENTRYPOINT [ "dotnet", "Hermes.Worker.dll", "--urls", "http://0.0.0.0:8000" ]