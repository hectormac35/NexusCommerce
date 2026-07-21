namespace Catalog.Application.Common.Errors;

public sealed record Error(
    string Codigo,
    string Mensaje,
    TipoError Tipo)
{
    public static readonly Error Ninguno = new(
        string.Empty,
        string.Empty,
        TipoError.Ninguno);

    public static Error Validacion(
        string codigo,
        string mensaje)
    {
        return new Error(
            codigo,
            mensaje,
            TipoError.Validacion);
    }

    public static Error NoEncontrado(
        string codigo,
        string mensaje)
    {
        return new Error(
            codigo,
            mensaje,
            TipoError.NoEncontrado);
    }

    public static Error Conflicto(
        string codigo,
        string mensaje)
    {
        return new Error(
            codigo,
            mensaje,
            TipoError.Conflicto);
    }

    public static Error NoAutorizado(
        string codigo,
        string mensaje)
    {
        return new Error(
            codigo,
            mensaje,
            TipoError.NoAutorizado);
    }

    public static Error Prohibido(
        string codigo,
        string mensaje)
    {
        return new Error(
            codigo,
            mensaje,
            TipoError.Prohibido);
    }

    public static Error Interno(
        string codigo,
        string mensaje)
    {
        return new Error(
            codigo,
            mensaje,
            TipoError.ErrorInterno);
    }
}
