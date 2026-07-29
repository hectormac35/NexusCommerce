using Identity.Application.Abstracciones.Persistencia;
using Identity.Application.Common.Resultados;
using MediatR;

namespace Identity.Application.Usuarios.CambiarRolUsuario;

internal sealed class CambiarRolUsuarioComandoHandler
    : IRequestHandler<CambiarRolUsuarioComando, Resultado>
{
    private readonly IRepositorioUsuarios _repositorio;

    public CambiarRolUsuarioComandoHandler(
        IRepositorioUsuarios repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<Resultado> Handle(
        CambiarRolUsuarioComando request,
        CancellationToken cancellationToken)
    {
        var usuario =
            await _repositorio.ObtenerParaActualizarAsync(
                request.UsuarioId,
                cancellationToken);

        if (usuario is null)
        {
            return Resultado.Fallo(
                ErroresCambiarRolUsuario.UsuarioNoEncontrado);
        }

        usuario.CambiarRol(request.NuevoRol);

        await _repositorio.GuardarCambiosAsync(
            cancellationToken);

        return Resultado.Exito();
    }
}
