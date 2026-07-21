namespace Identity.Domain.Common;

public sealed class ExcepcionDominio : Exception
{
    public ExcepcionDominio(string mensaje)
        : base(mensaje)
    {
    }
}
