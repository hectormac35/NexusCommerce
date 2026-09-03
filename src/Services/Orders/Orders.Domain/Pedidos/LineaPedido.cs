using Orders.Domain.Excepciones;

namespace Orders.Domain.Pedidos;

public sealed class LineaPedido
{
    private LineaPedido()
    {
    }

    internal LineaPedido(
        Guid productoId,
        string nombreProducto,
        decimal precioUnitario,
        int cantidad)
    {
        if (productoId == Guid.Empty)
        {
            throw new ExcepcionDominio(
                "El identificador del producto es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(nombreProducto))
        {
            throw new ExcepcionDominio(
                "El nombre del producto es obligatorio.");
        }

        if (precioUnitario <= 0)
        {
            throw new ExcepcionDominio(
                "El precio unitario debe ser mayor que cero.");
        }

        if (cantidad <= 0)
        {
            throw new ExcepcionDominio(
                "La cantidad debe ser mayor que cero.");
        }

        Id = Guid.NewGuid();
        ProductoId = productoId;
        NombreProducto = nombreProducto.Trim();
        PrecioUnitario = precioUnitario;
        Cantidad = cantidad;
    }

    public Guid Id { get; private set; }

    public Guid PedidoId { get; private set; }

    public Guid ProductoId { get; private set; }

    public string NombreProducto { get; private set; } = string.Empty;

    public decimal PrecioUnitario { get; private set; }

    public int Cantidad { get; private set; }

    public decimal Subtotal => PrecioUnitario * Cantidad;

    internal void IncrementarCantidad(int cantidad)
    {
        if (cantidad <= 0)
        {
            throw new ExcepcionDominio(
                "La cantidad que se desea añadir debe ser mayor que cero.");
        }

        Cantidad += cantidad;
    }
}
