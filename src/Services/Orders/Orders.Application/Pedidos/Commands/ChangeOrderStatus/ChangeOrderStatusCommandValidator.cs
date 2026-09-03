using FluentValidation;

namespace Orders.Application.Pedidos.Commands.ChangeOrderStatus;

public sealed class ChangeOrderStatusCommandValidator
    : AbstractValidator<ChangeOrderStatusCommand>
{
    private static readonly string[] EstadosPermitidos =
    [
        "Confirmado",
        "EnPreparacion",
        "Enviado",
        "Entregado",
        "Cancelado"
    ];

    public ChangeOrderStatusCommandValidator()
    {
        RuleFor(command => command.PedidoId)
            .NotEmpty();

        RuleFor(command => command.NuevoEstado)
            .NotEmpty()
            .Must(estado =>
                EstadosPermitidos.Contains(
                    estado,
                    StringComparer.OrdinalIgnoreCase))
            .WithMessage(
                "El estado debe ser Confirmado, EnPreparacion, Enviado, Entregado o Cancelado.");
    }
}
