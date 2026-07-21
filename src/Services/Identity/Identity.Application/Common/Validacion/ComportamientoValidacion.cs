using FluentValidation;
using MediatR;

namespace Identity.Application.Common.Validacion;

internal sealed class ComportamientoValidacion<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validadores)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validadores.Any())
        {
            return await next(cancellationToken);
        }

        var contexto = new ValidationContext<TRequest>(
            request);

        var resultados = await Task.WhenAll(
            validadores.Select(
                validador =>
                    validador.ValidateAsync(
                        contexto,
                        cancellationToken)));

        var errores = resultados
            .SelectMany(resultado => resultado.Errors)
            .Where(error => error is not null)
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                grupo => grupo.Key,
                grupo => grupo
                    .Select(error => error.ErrorMessage)
                    .Distinct()
                    .ToArray());

        if (errores.Count != 0)
        {
            throw new ExcepcionValidacion(errores);
        }

        return await next(cancellationToken);
    }
}
