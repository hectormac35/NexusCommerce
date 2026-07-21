using Identity.Application.Common.Resultados;
using MediatR;

namespace Identity.Application.Autenticacion.Registrar;

public sealed record RegistrarUsuarioComando(
    string Nombre,
    string Apellidos,
    string Correo,
    string Contrasena,
    string ConfirmacionContrasena)
    : IRequest<Resultado<UsuarioRegistradoRespuesta>>;

public sealed record UsuarioRegistradoRespuesta(
    Guid Id,
    string Nombre,
    string Apellidos,
    string Correo,
    string Rol);
