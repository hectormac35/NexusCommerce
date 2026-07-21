using Catalog.Application.Common.Exceptions;
using Catalog.Domain.Excepciones;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.ManejoErrores;

public sealed class ManejadorExcepcionesGlobal : IExceptionHandler
{
    private readonly ILogger<ManejadorExcepcionesGlobal> _logger;

    public ManejadorExcepcionesGlobal(
        ILogger<ManejadorExcepcionesGlobal> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var problema = exception switch
        {
            ExcepcionValidacion excepcionValidacion =>
                CrearProblemaValidacion(
                    httpContext,
                    excepcionValidacion),

            ExcepcionDominio excepcionDominio =>
                new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "La operación incumple una regla de negocio.",
                    Detail = excepcionDominio.Message,
                    Instance = httpContext.Request.Path
                },

            _ => CrearProblemaInterno(
                httpContext,
                exception)
        };

        problema.Extensions["traceId"] =
            httpContext.TraceIdentifier;

        httpContext.Response.StatusCode =
            problema.Status
            ?? StatusCodes.Status500InternalServerError;

        httpContext.Response.ContentType =
            "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(
            problema,
            cancellationToken);

        return true;
    }

    private static ProblemDetails CrearProblemaValidacion(
        HttpContext httpContext,
        ExcepcionValidacion excepcion)
    {
        var problema = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Se produjeron errores de validación.",
            Instance = httpContext.Request.Path
        };

        problema.Extensions["errors"] = excepcion.Errores;

        return problema;
    }

    private ProblemDetails CrearProblemaInterno(
        HttpContext httpContext,
        Exception exception)
    {
        _logger.LogError(
            exception,
            "Se produjo una excepción no controlada.");

        return new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Se produjo un error interno.",
            Detail = "No se pudo completar la operación.",
            Instance = httpContext.Request.Path
        };
    }
}
