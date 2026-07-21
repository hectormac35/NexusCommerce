namespace Catalog.Domain.Excepciones;

public sealed class ExcepcionDominio : Exception
{
    public ExcepcionDominio(string mensaje)
        : base(mensaje)
    {
    }
}
