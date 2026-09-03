using Orders.Domain.Excepciones;

namespace Orders.Domain.Pedidos;

public sealed class Pedido
{
    private readonly List<LineaPedido> _lineas = [];

    private Pedido()
    {
    }

    private Pedido(Guid clienteId)
    {
        if (clienteId == Guid.Empty)
        {
            throw new ExcepcionDominio(
                "El identificador del cliente es obligatorio.");
        }

        Id = Guid.NewGuid();
        ClienteId = clienteId;
        Estado = EstadoPedido.Pendiente;
        FechaCreacionUtc = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public Guid ClienteId { get; private set; }

    public EstadoPedido Estado { get; private set; }

    public DateTime FechaCreacionUtc { get; private set; }

    public DateTime? FechaActualizacionUtc { get; private set; }

    public DateTime? FechaCancelacionUtc { get; private set; }

    public IReadOnlyCollection<LineaPedido> Lineas => _lineas.AsReadOnly();

    public decimal Total => _lineas.Sum(linea => linea.Subtotal);

    public static Pedido Crear(Guid clienteId)
    {
        return new Pedido(clienteId);
    }

    public void AgregarLinea(
        Guid productoId,
        string nombreProducto,
        decimal precioUnitario,
        int cantidad)
    {
        VerificarQuePuedeModificarse();

        LineaPedido? lineaExistente = _lineas.FirstOrDefault(
            linea => linea.ProductoId == productoId);

        if (lineaExistente is not null)
        {
            if (lineaExistente.PrecioUnitario != precioUnitario)
            {
                throw new ExcepcionDominio(
                    "No se puede añadir el mismo producto con un precio diferente.");
            }

            lineaExistente.IncrementarCantidad(cantidad);
            MarcarComoActualizado();
            return;
        }

        var linea = new LineaPedido(
            productoId,
            nombreProducto,
            precioUnitario,
            cantidad);

        _lineas.Add(linea);
        MarcarComoActualizado();
    }

    public void EliminarLinea(Guid productoId)
    {
        VerificarQuePuedeModificarse();

        LineaPedido? linea = _lineas.FirstOrDefault(
            elemento => elemento.ProductoId == productoId);

        if (linea is null)
        {
            throw new ExcepcionDominio(
                "El producto no pertenece al pedido.");
        }

        _lineas.Remove(linea);
        MarcarComoActualizado();
    }

    public void Confirmar()
    {
        if (_lineas.Count == 0)
        {
            throw new ExcepcionDominio(
                "No se puede confirmar un pedido sin productos.");
        }

        CambiarEstado(
            EstadoPedido.Pendiente,
            EstadoPedido.Confirmado);
    }

    public void IniciarPreparacion()
    {
        CambiarEstado(
            EstadoPedido.Confirmado,
            EstadoPedido.EnPreparacion);
    }

    public void MarcarComoEnviado()
    {
        CambiarEstado(
            EstadoPedido.EnPreparacion,
            EstadoPedido.Enviado);
    }

    public void MarcarComoEntregado()
    {
        CambiarEstado(
            EstadoPedido.Enviado,
            EstadoPedido.Entregado);
    }

    public void Cancelar()
    {
        if (Estado is EstadoPedido.Enviado or EstadoPedido.Entregado)
        {
            throw new ExcepcionDominio(
                "No se puede cancelar un pedido enviado o entregado.");
        }

        if (Estado == EstadoPedido.Cancelado)
        {
            throw new ExcepcionDominio(
                "El pedido ya está cancelado.");
        }

        Estado = EstadoPedido.Cancelado;
        FechaCancelacionUtc = DateTime.UtcNow;
        MarcarComoActualizado();
    }

    private void CambiarEstado(
        EstadoPedido estadoEsperado,
        EstadoPedido estadoNuevo)
    {
        if (Estado != estadoEsperado)
        {
            throw new ExcepcionDominio(
                $"No se puede cambiar un pedido de {Estado} a {estadoNuevo}.");
        }

        Estado = estadoNuevo;
        MarcarComoActualizado();
    }

    private void VerificarQuePuedeModificarse()
    {
        if (Estado != EstadoPedido.Pendiente)
        {
            throw new ExcepcionDominio(
                "Solo se pueden modificar los productos de un pedido pendiente.");
        }
    }

    private void MarcarComoActualizado()
    {
        FechaActualizacionUtc = DateTime.UtcNow;
    }
}
