using FluentValidation;

namespace Identity.Application.Autenticacion.CerrarSesion;

public sealed class CerrarSesionComandoValidador
    : AbstractValidator<CerrarSesionComando>
{
    public CerrarSesionComandoValidador()
    {
        RuleFor(comando => comando.RefreshToken)
            .NotEmpty()
            .WithMessage(
                "El refresh token es obligatorio.");
    }
}
