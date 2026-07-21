using Identity.Application.Common.Resultados;
using MediatR;

namespace Identity.Application.Autenticacion.IniciarSesion;

public sealed record IniciarSesionComando(
    string Correo,
    string Contrasena)
    : IRequest<Resultado<SesionIniciadaRespuesta>>;

public sealed record SesionIniciadaRespuesta(
    string AccessToken,
    string RefreshToken,
    int ExpiraEnSegundos,
    UsuarioSesionRespuesta Usuario);

public sealed record UsuarioSesionRespuesta(
    Guid Id,
    string Nombre,
    string Apellidos,
    string Correo,
    string Rol);
