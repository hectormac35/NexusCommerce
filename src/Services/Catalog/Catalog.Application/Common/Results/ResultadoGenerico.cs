using Catalog.Application.Common.Errors;

namespace Catalog.Application.Common.Results;

public sealed class Resultado<T> : Resultado
{
    private readonly T? _valor;

    private Resultado(
        T? valor,
        bool esExitoso,
        Error error)
        : base(esExitoso, error)
    {
        _valor = valor;
    }

    public T Valor
    {
        get
        {
            if (EsFallo)
            {
                throw new InvalidOperationException(
                    "No se puede acceder al valor de un resultado fallido.");
            }

            return _valor!;
        }
    }

    public static Resultado<T> Exito(T valor)
    {
        return new Resultado<T>(
            valor,
            true,
            Error.Ninguno);
    }

    public static new Resultado<T> Fallo(Error error)
    {
        return new Resultado<T>(
            default,
            false,
            error);
    }

    public static implicit operator Resultado<T>(T valor)
    {
        return Exito(valor);
    }

    public static implicit operator Resultado<T>(Error error)
    {
        return Fallo(error);
    }
}
