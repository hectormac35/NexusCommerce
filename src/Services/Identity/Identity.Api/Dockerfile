FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY NexusCommerce.slnx ./
COPY global.json ./

COPY src/Services/Identity/Identity.Api/Identity.Api.csproj \
  src/Services/Identity/Identity.Api/

COPY src/Services/Identity/Identity.Application/Identity.Application.csproj \
  src/Services/Identity/Identity.Application/

COPY src/Services/Identity/Identity.Domain/Identity.Domain.csproj \
  src/Services/Identity/Identity.Domain/

COPY src/Services/Identity/Identity.Infrastructure/Identity.Infrastructure.csproj \
  src/Services/Identity/Identity.Infrastructure/

RUN dotnet restore \
  src/Services/Identity/Identity.Api/Identity.Api.csproj

COPY src/Services/Identity/ src/Services/Identity/

RUN dotnet publish \
  src/Services/Identity/Identity.Api/Identity.Api.csproj \
  -c Release \
  -o /app/publish \
  --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0

RUN apt-get update \
 && apt-get install -y --no-install-recommends curl \
 && rm -rf /var/lib/apt/lists/*

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet","Identity.Api.dll"]
