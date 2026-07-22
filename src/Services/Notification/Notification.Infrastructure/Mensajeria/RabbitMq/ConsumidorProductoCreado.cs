using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Notification.Application.Eventos.Integracion;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Notification.Infrastructure.Mensajeria.RabbitMq;

public sealed class ConsumidorProductoCreado(
    IOptions<RabbitMqOpciones> opciones,
    ILogger<ConsumidorProductoCreado> logger)
    : BackgroundService
{
    private readonly RabbitMqOpciones _opciones =
        opciones.Value;

    private IConnection? _conexion;
    private IChannel? _canal;

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        var fabrica = new ConnectionFactory
        {
            HostName = _opciones.Host,
            Port = _opciones.Puerto,
            UserName = _opciones.Usuario,
            Password = _opciones.Contrasena,
            VirtualHost = _opciones.VirtualHost,
            ClientProvidedName =
                "NexusCommerce.Notification.Worker"
        };

        _conexion = await fabrica.CreateConnectionAsync(
            stoppingToken);

        _canal = await _conexion.CreateChannelAsync(
            cancellationToken: stoppingToken);

        await _canal.ExchangeDeclareAsync(
            exchange: _opciones.Exchange,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false,
            cancellationToken: stoppingToken);

        await _canal.QueueDeclareAsync(
            queue: _opciones.Cola,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: stoppingToken);

        await _canal.QueueBindAsync(
            queue: _opciones.Cola,
            exchange: _opciones.Exchange,
            routingKey: _opciones.RoutingKey,
            cancellationToken: stoppingToken);

        await _canal.BasicQosAsync(
            prefetchSize: 0,
            prefetchCount: 1,
            global: false,
            cancellationToken: stoppingToken);

        var consumidor =
            new AsyncEventingBasicConsumer(_canal);

        consumidor.ReceivedAsync += ProcesarMensajeAsync;

        await _canal.BasicConsumeAsync(
            queue: _opciones.Cola,
            autoAck: false,
            consumer: consumidor,
            cancellationToken: stoppingToken);

        logger.LogInformation(
            "Consumidor RabbitMQ iniciado. Cola: {Cola}. RoutingKey: {RoutingKey}",
            _opciones.Cola,
            _opciones.RoutingKey);

        try
        {
            await Task.Delay(
                Timeout.Infinite,
                stoppingToken);
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation(
                "Consumidor RabbitMQ detenido.");
        }
    }

    private async Task ProcesarMensajeAsync(
        object sender,
        BasicDeliverEventArgs args)
    {
        if (_canal is null)
        {
            return;
        }

        try
        {
            var contenido = Encoding.UTF8.GetString(
                args.Body.ToArray());

            var evento =
                JsonSerializer.Deserialize<ProductoCreadoEvento>(
                    contenido,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (evento is null)
            {
                throw new JsonException(
                    "El mensaje recibido está vacío.");
            }

            logger.LogInformation(
                """
                Producto creado recibido:
                EventoId: {EventoId}
                ProductoId: {ProductoId}
                Nombre: {Nombre}
                Precio: {Precio}
                Stock: {Stock}
                Categoría: {Categoria}
                OcurridoEnUtc: {OcurridoEnUtc}
                """,
                evento.EventoId,
                evento.ProductoId,
                evento.Nombre,
                evento.Precio,
                evento.Stock,
                evento.Categoria,
                evento.OcurridoEnUtc);

            await _canal.BasicAckAsync(
                args.DeliveryTag,
                multiple: false);
        }
        catch (JsonException excepcion)
        {
            logger.LogError(
                excepcion,
                "El mensaje recibido no contiene un evento válido.");

            await _canal.BasicNackAsync(
                args.DeliveryTag,
                multiple: false,
                requeue: false);
        }
        catch (Exception excepcion)
        {
            logger.LogError(
                excepcion,
                "Error procesando ProductoCreadoEvento.");

            await _canal.BasicNackAsync(
                args.DeliveryTag,
                multiple: false,
                requeue: true);
        }
    }

    public override async Task StopAsync(
        CancellationToken cancellationToken)
    {
        if (_canal is not null)
        {
            await _canal.CloseAsync(cancellationToken);
            await _canal.DisposeAsync();
        }

        if (_conexion is not null)
        {
            await _conexion.CloseAsync(cancellationToken);
            await _conexion.DisposeAsync();
        }

        await base.StopAsync(cancellationToken);
    }
}
