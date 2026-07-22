namespace Catalog.Infrastructure.Mensajeria.RabbitMq;

public sealed class RabbitMqOpciones
{
    public const string Seccion = "RabbitMq";

    public string Host { get; init; } = "localhost";

    public int Puerto { get; init; } = 5672;

    public string Usuario { get; init; } = "nexus";

    public string Contrasena { get; init; } = string.Empty;

    public string VirtualHost { get; init; } = "/";

    public string Exchange { get; init; } =
        "nexuscommerce.eventos";

    public string NombreCliente { get; init; } =
        "NexusCommerce.Catalog";
}
