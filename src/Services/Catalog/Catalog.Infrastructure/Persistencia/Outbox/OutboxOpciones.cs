namespace Catalog.Infrastructure.Persistencia.Outbox;

public sealed class OutboxOpciones
{
    public const string Seccion = "Outbox";

    public bool Habilitado { get; init; }

    public int IntervaloSegundos { get; init; } = 5;

    public int TamanoLote { get; init; } = 20;

    public int MaximoIntentos { get; init; } = 10;
}
