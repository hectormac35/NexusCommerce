using Identity.Application.Common.Resultados;
using MediatR;

namespace Identity.Application.Usuarios.ObtenerUsuarioActual;

public sealed record ObtenerUsuarioActualConsulta(
    Guid UsuarioId)
    : IRequest<Resultado<UsuarioActualRespuesta>>;
