FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY NexusCommerce.slnx ./
COPY global.json ./

COPY src/Services/Catalog/Catalog.Api/Catalog.Api.csproj \
  src/Services/Catalog/Catalog.Api/

COPY src/Services/Catalog/Catalog.Application/Catalog.Application.csproj \
  src/Services/Catalog/Catalog.Application/

COPY src/Services/Catalog/Catalog.Domain/Catalog.Domain.csproj \
  src/Services/Catalog/Catalog.Domain/

COPY src/Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj \
  src/Services/Catalog/Catalog.Infrastructure/

RUN dotnet restore \
  src/Services/Catalog/Catalog.Api/Catalog.Api.csproj

COPY src/Services/Catalog/ src/Services/Catalog/

RUN dotnet publish \
  src/Services/Catalog/Catalog.Api/Catalog.Api.csproj \
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

ENTRYPOINT ["dotnet", "Catalog.Api.dll"]
