using FluentValidation;

namespace Orders.Application.Pedidos.Commands.CreateOrder;

public sealed class CreateOrderCommandValidator
    : AbstractValidator<CreateOrderCommand>
{
    public const int MaximoLineas = 50;
    public const int MaximoUnidadesPorLinea = 100;

    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.ClienteId)
            .NotEmpty();

        RuleFor(x => x.Lineas)
            .NotEmpty()
            .Must(lineas => lineas.Count <= MaximoLineas)
            .WithMessage($"Un pedido no puede tener más de {MaximoLineas} líneas.");

        RuleForEach(x => x.Lineas)
            .ChildRules(linea =>
            {
                linea.RuleFor(x => x.ProductoId)
                    .NotEmpty();

                linea.RuleFor(x => x.Cantidad)
                    .InclusiveBetween(1, MaximoUnidadesPorLinea);
            });
    }
}
