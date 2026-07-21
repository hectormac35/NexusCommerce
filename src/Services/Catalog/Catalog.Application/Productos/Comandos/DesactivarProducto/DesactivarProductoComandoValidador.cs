using FluentValidation;

namespace Catalog.Application.Productos.Comandos.DesactivarProducto;

public sealed class DesactivarProductoComandoValidador
    : AbstractValidator<DesactivarProductoComando>
{
    public DesactivarProductoComandoValidador()
    {
        RuleFor(comando => comando.Id)
            .NotEmpty()
            .WithMessage("El identificador del producto es obligatorio.");
    }
}
