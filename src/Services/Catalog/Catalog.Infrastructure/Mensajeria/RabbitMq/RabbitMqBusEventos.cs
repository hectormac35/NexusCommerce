using System.Text;
using System.Text.Json;
using Catalog.Application.Abstracciones.Mensajeria;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Catalog.Infrastructure.Mensajeria.RabbitMq;

internal sealed class RabbitMqBusEventos(
    IOptions<RabbitMqOpciones> opciones)
    : IBusEventos
{
    private readonly RabbitMqOpciones _opciones =
        opciones.Value;

    public async Task PublicarAsync<TEvento>(
        TEvento evento,
        CancellationToken cancellationToken = default)
        where TEvento : IEventoIntegracion
    {
        var factory = new ConnectionFactory
        {
            HostName = _opciones.Host,
            Port = _opciones.Puerto,
            UserName = _opciones.Usuario,
            Password = _opciones.Contrasena,
            VirtualHost = _opciones.VirtualHost,
            ClientProvidedName = _opciones.NombreCliente
        };

        await using var conexion =
            await factory.CreateConnectionAsync(
                cancellationToken);

        await using var canal =
            await conexion.CreateChannelAsync(
                cancellationToken: cancellationToken);

        await canal.ExchangeDeclareAsync(
            exchange: _opciones.Exchange,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false,
            cancellationToken: cancellationToken);

        var nombreEvento = typeof(TEvento).Name;

        var contenido = JsonSerializer.Serialize(
            evento,
            evento.GetType());

        var cuerpo = Encoding.UTF8.GetBytes(contenido);

        var propiedades =
            new BasicProperties
            {
                ContentType = "application/json",
                DeliveryMode =
                    DeliveryModes.Persistent,
                MessageId =
                    evento.EventoId.ToString(),
                Type = nombreEvento,
                Timestamp =
                    new AmqpTimestamp(
                        new DateTimeOffset(
                            evento.OcurridoEnUtc)
                        .ToUnixTimeSeconds())
            };

        await canal.BasicPublishAsync(
            exchange: _opciones.Exchange,
            routingKey: nombreEvento,
            mandatory: false,
            basicProperties: propiedades,
            body: cuerpo,
            cancellationToken: cancellationToken);
    }
}
