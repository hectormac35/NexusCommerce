namespace Catalog.Infrastructure.Persistencia.Outbox;

public sealed class MensajeOutbox
{
    private MensajeOutbox()
    {
    }

    public MensajeOutbox(
        Guid id,
        DateTime ocurridoEnUtc,
        string tipo,
        string contenido)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "El identificador del mensaje es obligatorio.",
                nameof(id));
        }

        if (string.IsNullOrWhiteSpace(tipo))
        {
            throw new ArgumentException(
                "El tipo del mensaje es obligatorio.",
                nameof(tipo));
        }

        if (string.IsNullOrWhiteSpace(contenido))
        {
            throw new ArgumentException(
                "El contenido del mensaje es obligatorio.",
                nameof(contenido));
        }

        Id = id;
        OcurridoEnUtc = ocurridoEnUtc;
        Tipo = tipo;
        Contenido = contenido;
    }

    public Guid Id { get; private set; }

    public DateTime OcurridoEnUtc { get; private set; }

    public string Tipo { get; private set; } = string.Empty;

    public string Contenido { get; private set; } = string.Empty;

    public DateTime? ProcesadoEnUtc { get; private set; }

    public DateTime? UltimoIntentoEnUtc { get; private set; }

    public int Intentos { get; private set; }

    public string? Error { get; private set; }

    public void MarcarProcesado(DateTime procesadoEnUtc)
    {
        ProcesadoEnUtc = procesadoEnUtc;
        UltimoIntentoEnUtc = procesadoEnUtc;
        Error = null;
    }

    public void RegistrarError(
        string error,
        DateTime intentoEnUtc)
    {
        Intentos++;
        UltimoIntentoEnUtc = intentoEnUtc;
        Error = error.Length <= 2000
            ? error
            : error[..2000];
    }
}
