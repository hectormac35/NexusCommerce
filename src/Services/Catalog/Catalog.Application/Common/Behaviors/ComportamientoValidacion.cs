using Catalog.Application.Common.Exceptions;
using FluentValidation;
using MediatR;

namespace Catalog.Application.Common.Behaviors;

internal sealed class ComportamientoValidacion<TSolicitud, TRespuesta>
    : IPipelineBehavior<TSolicitud, TRespuesta>
    where TSolicitud : notnull
{
    private readonly IEnumerable<IValidator<TSolicitud>> _validadores;

    public ComportamientoValidacion(
        IEnumerable<IValidator<TSolicitud>> validadores)
    {
        _validadores = validadores;
    }

    public async Task<TRespuesta> Handle(
        TSolicitud solicitud,
        RequestHandlerDelegate<TRespuesta> siguiente,
        CancellationToken cancellationToken)
    {
        if (!_validadores.Any())
        {
            return await siguiente();
        }

        var contexto = new ValidationContext<TSolicitud>(solicitud);

        var resultados = await Task.WhenAll(
            _validadores.Select(validador =>
                validador.ValidateAsync(
                    contexto,
                    cancellationToken)));

        var errores = resultados
            .SelectMany(resultado => resultado.Errors)
            .Where(error => error is not null)
            .ToArray();

        if (errores.Length > 0)
        {
            throw new ExcepcionValidacion(errores);
        }

        return await siguiente();
    }
}
