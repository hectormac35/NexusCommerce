namespace Identity.Application.Common.Validacion;

public sealed class ExcepcionValidacion(
    IReadOnlyDictionary<string, string[]> errores)
    : Exception("Se produjeron uno o varios errores de validación.")
{
    public IReadOnlyDictionary<string, string[]> Errores
    {
        get;
    } = errores;
}
