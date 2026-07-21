namespace Identity.Application.Common.Resultados;

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

    public static Resultado Exito() =>
        new(true, Error.Ninguno);

    public static Resultado Fallo(Error error) =>
        new(false, error);
}

public sealed class Resultado<T> : Resultado
{
    private Resultado(
        T? valor,
        bool esExitoso,
        Error error)
        : base(esExitoso, error)
    {
        Valor = valor;
    }

    public T? Valor { get; }

    public static Resultado<T> Exito(T valor) =>
        new(valor, true, Error.Ninguno);

    public new static Resultado<T> Fallo(Error error) =>
        new(default, false, error);
}
