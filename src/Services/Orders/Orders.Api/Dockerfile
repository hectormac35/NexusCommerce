FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY NexusCommerce.slnx ./
COPY global.json ./

COPY src/Services/Orders/Orders.Api/Orders.Api.csproj \
  src/Services/Orders/Orders.Api/

COPY src/Services/Orders/Orders.Application/Orders.Application.csproj \
  src/Services/Orders/Orders.Application/

COPY src/Services/Orders/Orders.Domain/Orders.Domain.csproj \
  src/Services/Orders/Orders.Domain/

COPY src/Services/Orders/Orders.Infrastructure/Orders.Infrastructure.csproj \
  src/Services/Orders/Orders.Infrastructure/

RUN dotnet restore \
  src/Services/Orders/Orders.Api/Orders.Api.csproj

COPY src/Services/Orders/ src/Services/Orders/

RUN dotnet publish \
  src/Services/Orders/Orders.Api/Orders.Api.csproj \
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

ENTRYPOINT ["dotnet", "Orders.Api.dll"]
