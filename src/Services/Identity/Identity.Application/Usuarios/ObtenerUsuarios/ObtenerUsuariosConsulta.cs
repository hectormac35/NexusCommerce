using MediatR;

namespace Identity.Application.Usuarios.ObtenerUsuarios;

public sealed record ObtenerUsuariosConsulta
    : IRequest<IReadOnlyList<UsuarioListadoRespuesta>>;
