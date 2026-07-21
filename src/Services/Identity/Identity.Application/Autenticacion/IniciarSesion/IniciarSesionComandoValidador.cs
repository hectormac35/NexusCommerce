using FluentValidation;

namespace Identity.Application.Autenticacion.IniciarSesion;

public sealed class IniciarSesionComandoValidador
    : AbstractValidator<IniciarSesionComando>
{
    public IniciarSesionComandoValidador()
    {
        RuleFor(comando => comando.Correo)
            .NotEmpty()
            .WithMessage("El correo electrónico es obligatorio.")
            .EmailAddress()
            .WithMessage(
                "El correo electrónico no tiene un formato válido.");

        RuleFor(comando => comando.Contrasena)
            .NotEmpty()
            .WithMessage("La contraseña es obligatoria.");
    }
}
