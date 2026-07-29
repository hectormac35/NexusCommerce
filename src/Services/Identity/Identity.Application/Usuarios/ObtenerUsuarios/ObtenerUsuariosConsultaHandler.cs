using Identity.Application.Abstracciones.Persistencia;
using MediatR;

namespace Identity.Application.Usuarios.ObtenerUsuarios;

internal sealed class ObtenerUsuariosConsultaHandler(
    IRepositorioUsuarios repositorio)
    : IRequestHandler<
        ObtenerUsuariosConsulta,
        IReadOnlyList<UsuarioListadoRespuesta>>
{
    public async Task<IReadOnlyList<UsuarioListadoRespuesta>> Handle(
        ObtenerUsuariosConsulta request,
        CancellationToken cancellationToken)
    {
        var usuarios = await repositorio.ObtenerTodosAsync(
            cancellationToken);

        return usuarios
            .Select(usuario => new UsuarioListadoRespuesta(
                usuario.Id,
                usuario.Nombre,
                usuario.Apellidos,
                usuario.NombreCompleto,
                usuario.Correo,
                usuario.Rol.ToString(),
                usuario.Estado.ToString(),
                usuario.EstaActivo,
                usuario.FechaCreacionUtc,
                usuario.FechaActualizacionUtc))
            .ToList();
    }
}
