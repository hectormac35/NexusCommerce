FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY NexusCommerce.slnx ./
COPY global.json ./

COPY src/Services/Notification/Notification.Worker/Notification.Worker.csproj \
  src/Services/Notification/Notification.Worker/

COPY src/Services/Notification/Notification.Application/Notification.Application.csproj \
  src/Services/Notification/Notification.Application/

COPY src/Services/Notification/Notification.Infrastructure/Notification.Infrastructure.csproj \
  src/Services/Notification/Notification.Infrastructure/

RUN dotnet restore \
  src/Services/Notification/Notification.Worker/Notification.Worker.csproj

COPY src/Services/Notification/ src/Services/Notification/

RUN dotnet publish \
  src/Services/Notification/Notification.Worker/Notification.Worker.csproj \
  -c Release \
  -o /app/publish \
  --no-restore

FROM mcr.microsoft.com/dotnet/runtime:10.0

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet","Notification.Worker.dll"]
