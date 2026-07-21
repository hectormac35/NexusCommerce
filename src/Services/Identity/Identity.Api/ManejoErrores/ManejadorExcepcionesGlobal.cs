using Identity.Application.Common.Validacion;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.ManejoErrores;

internal sealed class ManejadorExcepcionesGlobal
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is ExcepcionValidacion validacion)
        {
            var problema = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validacion.Error",
                Detail =
                    "Se produjeron errores de validación.",
                Instance = httpContext.Request.Path
            };

            problema.Extensions["errors"] =
                validacion.Errores;

            httpContext.Response.StatusCode =
                StatusCodes.Status400BadRequest;

            await httpContext.Response.WriteAsJsonAsync(
                problema,
                cancellationToken);

            return true;
        }

        return false;
    }
}
