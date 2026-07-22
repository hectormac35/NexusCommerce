using Identity.Application.Common.Resultados;
using MediatR;

namespace Identity.Application.Autenticacion.CerrarSesion;

public sealed record CerrarSesionComando(
    string RefreshToken)
    : IRequest<Resultado>;
