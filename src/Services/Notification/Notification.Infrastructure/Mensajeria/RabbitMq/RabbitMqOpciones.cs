namespace Notification.Infrastructure.Mensajeria.RabbitMq;

public sealed class RabbitMqOpciones
{
    public const string Seccion = "RabbitMq";

    public string Host { get; init; } = "localhost";

    public int Puerto { get; init; } = 5672;

    public string Usuario { get; init; } = "guest";

    public string Contrasena { get; init; } = "guest";

    public string VirtualHost { get; init; } = "/";

    public string Exchange { get; init; } =
        "nexuscommerce.eventos";

    public string Cola { get; init; } =
        "nexuscommerce.notificaciones.productos";

    public string RoutingKey { get; init; } =
        "ProductoCreadoEvento";
}
