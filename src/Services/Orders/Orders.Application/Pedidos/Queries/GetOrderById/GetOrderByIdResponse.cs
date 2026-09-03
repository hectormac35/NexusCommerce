namespace Orders.Application.Pedidos.Queries.GetOrderById;

public sealed class GetOrderByIdResponse
{
    public Guid PedidoId { get; init; }

    public Guid ClienteId { get; init; }

    public string Estado { get; init; } = string.Empty;

    public decimal Total { get; init; }

    public DateTime FechaCreacionUtc { get; init; }

    public IReadOnlyCollection<GetOrderLineResponse> Lineas { get; init; }
        = [];

    public sealed class GetOrderLineResponse
    {
        public Guid ProductoId { get; init; }

        public string NombreProducto { get; init; } = string.Empty;

        public decimal PrecioUnitario { get; init; }

        public int Cantidad { get; init; }

        public decimal Subtotal => PrecioUnitario * Cantidad;
    }
}
