namespace Identity.Application.Common.Resultados;

public enum TipoError
{
    Ninguno = 0,
    Validacion = 1,
    Conflicto = 2,
    NoEncontrado = 3,
    NoAutorizado = 4,
    Inesperado = 5
}

public sealed record Error(
    string Codigo,
    string Mensaje,
    TipoError Tipo)
{
    public static readonly Error Ninguno =
        new(string.Empty, string.Empty, TipoError.Ninguno);
}
