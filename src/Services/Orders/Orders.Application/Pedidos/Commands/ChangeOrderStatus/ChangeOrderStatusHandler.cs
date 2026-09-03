using Orders.Application.Abstractions.Messaging;
using Orders.Application.Abstractions.Persistence;
using Orders.Application.Common.Results;
using Orders.Domain.Pedidos;

namespace Orders.Application.Pedidos.Commands.ChangeOrderStatus;

public sealed class ChangeOrderStatusHandler
    : ICommandHandler<ChangeOrderStatusCommand>
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeOrderStatusHandler(
        IPedidoRepository pedidoRepository,
        IUnitOfWork unitOfWork)
    {
        _pedidoRepository = pedidoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        ChangeOrderStatusCommand request,
        CancellationToken cancellationToken)
    {
        Pedido? pedido = await _pedidoRepository.ObtenerPorIdAsync(
            request.PedidoId,
            cancellationToken);

        if (pedido is null)
        {
            return Result.Fail("Pedido no encontrado.");
        }

        switch (request.NuevoEstado.Trim().ToLowerInvariant())
        {
            case "confirmado":
                pedido.Confirmar();
                break;

            case "enpreparacion":
                pedido.IniciarPreparacion();
                break;

            case "enviado":
                pedido.MarcarComoEnviado();
                break;

            case "entregado":
                pedido.MarcarComoEntregado();
                break;

            case "cancelado":
                pedido.Cancelar();
                break;

            default:
                return Result.Fail("Estado no válido.");
        }

        await _unitOfWork.GuardarCambiosAsync(
            cancellationToken);

        return Result.Ok();
    }
}
