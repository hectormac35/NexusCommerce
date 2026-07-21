FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY NexusCommerce.slnx ./
COPY global.json ./

COPY src/Gateway/NexusCommerce.Gateway/NexusCommerce.Gateway.csproj \
  src/Gateway/NexusCommerce.Gateway/

RUN dotnet restore \
  src/Gateway/NexusCommerce.Gateway/NexusCommerce.Gateway.csproj

COPY src/Gateway/NexusCommerce.Gateway/ \
  src/Gateway/NexusCommerce.Gateway/

RUN dotnet publish \
  src/Gateway/NexusCommerce.Gateway/NexusCommerce.Gateway.csproj \
  --configuration Release \
  --output /app/publish \
  --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "NexusCommerce.Gateway.dll"]
