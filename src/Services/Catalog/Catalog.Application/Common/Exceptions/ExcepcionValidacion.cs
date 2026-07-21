using FluentValidation.Results;

namespace Catalog.Application.Common.Exceptions;

public sealed class ExcepcionValidacion : Exception
{
    public ExcepcionValidacion(
        IReadOnlyCollection<ValidationFailure> errores)
        : base("Se produjeron uno o más errores de validación.")
    {
        Errores = errores
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                grupo => grupo.Key,
                grupo => grupo
                    .Select(error => error.ErrorMessage)
                    .Distinct()
                    .ToArray());
    }

    public IReadOnlyDictionary<string, string[]> Errores { get; }
}
