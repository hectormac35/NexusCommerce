using FluentValidation;

namespace Catalog.Application.Productos.Comandos.CrearProducto;

public sealed class CrearProductoComandoValidador
    : AbstractValidator<CrearProductoComando>
{
    public CrearProductoComandoValidador()
    {
        RuleFor(comando => comando.Nombre)
            .NotEmpty()
            .WithMessage("El nombre del producto es obligatorio.")
            .MaximumLength(150)
            .WithMessage("El nombre no puede superar los 150 caracteres.");

        RuleFor(comando => comando.Descripcion)
            .NotEmpty()
            .WithMessage("La descripción del producto es obligatoria.")
            .MaximumLength(1_000)
            .WithMessage("La descripción no puede superar los 1000 caracteres.");

        RuleFor(comando => comando.Precio)
            .GreaterThan(0)
            .WithMessage("El precio debe ser mayor que cero.");

        RuleFor(comando => comando.Stock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El stock no puede ser negativo.");

        RuleFor(comando => comando.Categoria)
            .NotEmpty()
            .WithMessage("La categoría del producto es obligatoria.")
            .MaximumLength(100)
            .WithMessage("La categoría no puede superar los 100 caracteres.");
    }
}
