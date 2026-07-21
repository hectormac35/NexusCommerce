using FluentValidation;

namespace Identity.Application.Autenticacion.Registrar;

public sealed class RegistrarUsuarioComandoValidador
    : AbstractValidator<RegistrarUsuarioComando>
{
    public RegistrarUsuarioComandoValidador()
    {
        RuleFor(comando => comando.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio.")
            .MaximumLength(100)
            .WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(comando => comando.Apellidos)
            .NotEmpty()
            .WithMessage("Los apellidos son obligatorios.")
            .MaximumLength(150)
            .WithMessage(
                "Los apellidos no pueden superar los 150 caracteres.");

        RuleFor(comando => comando.Correo)
            .NotEmpty()
            .WithMessage("El correo electrónico es obligatorio.")
            .EmailAddress()
            .WithMessage(
                "El correo electrónico no tiene un formato válido.")
            .MaximumLength(320);

        RuleFor(comando => comando.Contrasena)
            .NotEmpty()
            .WithMessage("La contraseña es obligatoria.")
            .MinimumLength(10)
            .WithMessage(
                "La contraseña debe contener al menos 10 caracteres.")
            .Matches("[A-Z]")
            .WithMessage(
                "La contraseña debe contener una letra mayúscula.")
            .Matches("[a-z]")
            .WithMessage(
                "La contraseña debe contener una letra minúscula.")
            .Matches("[0-9]")
            .WithMessage(
                "La contraseña debe contener un número.")
            .Matches("[^a-zA-Z0-9]")
            .WithMessage(
                "La contraseña debe contener un carácter especial.");

        RuleFor(comando => comando.ConfirmacionContrasena)
            .Equal(comando => comando.Contrasena)
            .WithMessage("Las contraseñas no coinciden.");
    }
}
