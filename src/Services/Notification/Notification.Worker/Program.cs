using Notification.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AgregarInfraestructuraNotificaciones(
    builder.Configuration);

var host = builder.Build();

host.Run();
