using FluentValidation;

namespace Identity.Application.Usuarios.CambiarRolUsuario;

public sealed class CambiarRolUsuarioComandoValidador
    : AbstractValidator<CambiarRolUsuarioComando>
{
    public CambiarRolUsuarioComandoValidador()
    {
        RuleFor(comando => comando.UsuarioId)
            .NotEmpty()
            .WithMessage(
                "El identificador del usuario es obligatorio.");

        RuleFor(comando => comando.NuevoRol)
            .IsInEnum()
            .WithMessage(
                "El rol indicado no es válido.");
    }
}
