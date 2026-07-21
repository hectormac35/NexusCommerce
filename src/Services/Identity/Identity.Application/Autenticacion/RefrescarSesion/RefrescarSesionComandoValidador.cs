using FluentValidation;

namespace Identity.Application.Autenticacion.RefrescarSesion;

public sealed class RefrescarSesionComandoValidador
    : AbstractValidator<RefrescarSesionComando>
{
    public RefrescarSesionComandoValidador()
    {
        RuleFor(comando => comando.RefreshToken)
            .NotEmpty()
            .WithMessage(
                "El refresh token es obligatorio.");
    }
}
