using Identity.Application.Abstracciones.Persistencia;
using Identity.Application.Common.Resultados;
using MediatR;

namespace Identity.Application.Usuarios.ObtenerUsuarioActual;

internal sealed class ObtenerUsuarioActualConsultaHandler
    : IRequestHandler<
        ObtenerUsuarioActualConsulta,
        Resultado<UsuarioActualRespuesta>>
{
    private readonly IRepositorioUsuarios _repositorio;

    public ObtenerUsuarioActualConsultaHandler(
        IRepositorioUsuarios repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<Resultado<UsuarioActualRespuesta>> Handle(
        ObtenerUsuarioActualConsulta request,
        CancellationToken cancellationToken)
    {
        var usuario = await _repositorio.ObtenerPorIdAsync(
            request.UsuarioId,
            cancellationToken);

        if (usuario is null)
        {
            return Resultado<UsuarioActualRespuesta>.Fallo(
                ErroresObtenerUsuarioActual.UsuarioNoEncontrado);
        }

        if (!usuario.EstaActivo)
        {
            return Resultado<UsuarioActualRespuesta>.Fallo(
                ErroresObtenerUsuarioActual.UsuarioInactivo);
        }

        var respuesta = new UsuarioActualRespuesta(
            usuario.Id,
            usuario.Nombre,
            usuario.Apellidos,
            usuario.Correo,
            usuario.Rol.ToString());

        return Resultado<UsuarioActualRespuesta>.Exito(
            respuesta);
    }
}
