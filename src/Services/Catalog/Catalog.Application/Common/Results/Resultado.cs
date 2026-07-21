using Catalog.Application.Common.Errors;

namespace Catalog.Application.Common.Results;

public class Resultado
{
    protected Resultado(
        bool esExitoso,
        Error error)
    {
        if (esExitoso && error != Error.Ninguno)
        {
            throw new InvalidOperationException(
                "Un resultado exitoso no puede contener un error.");
        }

        if (!esExitoso && error == Error.Ninguno)
        {
            throw new InvalidOperationException(
                "Un resultado fallido debe contener un error.");
        }

        EsExitoso = esExitoso;
        Error = error;
    }

    public bool EsExitoso { get; }

    public bool EsFallo => !EsExitoso;

    public Error Error { get; }

    public static Resultado Exito()
    {
        return new Resultado(
            true,
            Error.Ninguno);
    }

    public static Resultado Fallo(Error error)
    {
        return new Resultado(
            false,
            error);
    }
}
