using Identity.Application.Common.Resultados;
using Identity.Domain.Usuarios;
using MediatR;

namespace Identity.Application.Usuarios.CambiarRolUsuario;

public sealed record CambiarRolUsuarioComando(
    Guid UsuarioId,
    RolUsuario NuevoRol)
    : IRequest<Resultado>;
