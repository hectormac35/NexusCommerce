using Identity.Application.Autenticacion.IniciarSesion;
using Identity.Application.Common.Resultados;
using MediatR;

namespace Identity.Application.Autenticacion.RefrescarSesion;

public sealed record RefrescarSesionComando(
    string RefreshToken)
    : IRequest<Resultado<SesionIniciadaRespuesta>>;
