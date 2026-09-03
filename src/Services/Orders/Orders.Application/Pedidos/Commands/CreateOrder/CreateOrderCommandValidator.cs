using FluentValidation;

namespace Orders.Application.Pedidos.Commands.CreateOrder;

public sealed class CreateOrderCommandValidator
    : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.ClienteId)
            .NotEmpty();

        RuleFor(x => x.Lineas)
            .NotEmpty();

        RuleForEach(x => x.Lineas)
            .ChildRules(linea =>
            {
                linea.RuleFor(x => x.ProductoId)
                    .NotEmpty();

                linea.RuleFor(x => x.NombreProducto)
                    .NotEmpty()
                    .MaximumLength(200);

                linea.RuleFor(x => x.PrecioUnitario)
                    .GreaterThan(0);

                linea.RuleFor(x => x.Cantidad)
                    .GreaterThan(0);
            });
    }
}
