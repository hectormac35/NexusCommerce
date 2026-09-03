using Orders.Domain.Excepciones;
using Orders.Domain.Pedidos;

namespace Orders.Domain.UnitTests.Pedidos;

public sealed class PedidoTests
{
    [Fact]
    public void Crear_ConClienteValido_CreaPedidoPendiente()
    {
        Guid clienteId = Guid.NewGuid();

        Pedido pedido = Pedido.Crear(clienteId);

        Assert.NotEqual(Guid.Empty, pedido.Id);
        Assert.Equal(clienteId, pedido.ClienteId);
        Assert.Equal(EstadoPedido.Pendiente, pedido.Estado);
        Assert.Empty(pedido.Lineas);
        Assert.Equal(0m, pedido.Total);
    }

    [Fact]
    public void Crear_ConClienteVacio_LanzaExcepcion()
    {
        Action accion = () => Pedido.Crear(Guid.Empty);

        Assert.Throws<ExcepcionDominio>(accion);
    }

    [Fact]
    public void AgregarLinea_ConDatosValidos_CalculaElTotal()
    {
        Pedido pedido = Pedido.Crear(Guid.NewGuid());

        pedido.AgregarLinea(
            Guid.NewGuid(),
            "Teclado mecánico",
            79.99m,
            2);

        Assert.Single(pedido.Lineas);
        Assert.Equal(159.98m, pedido.Total);
    }

    [Fact]
    public void AgregarMismoProducto_IncrementaLaCantidad()
    {
        Pedido pedido = Pedido.Crear(Guid.NewGuid());
        Guid productoId = Guid.NewGuid();

        pedido.AgregarLinea(
            productoId,
            "Ratón inalámbrico",
            39.50m,
            1);

        pedido.AgregarLinea(
            productoId,
            "Ratón inalámbrico",
            39.50m,
            2);

        LineaPedido linea = Assert.Single(pedido.Lineas);

        Assert.Equal(3, linea.Cantidad);
        Assert.Equal(118.50m, pedido.Total);
    }

    [Fact]
    public void Confirmar_SinProductos_LanzaExcepcion()
    {
        Pedido pedido = Pedido.Crear(Guid.NewGuid());

        Action accion = pedido.Confirmar;

        Assert.Throws<ExcepcionDominio>(accion);
        Assert.Equal(EstadoPedido.Pendiente, pedido.Estado);
    }

    [Fact]
    public void Confirmar_ConProductos_CambiaElEstado()
    {
        Pedido pedido = CrearPedidoConProducto();

        pedido.Confirmar();

        Assert.Equal(EstadoPedido.Confirmado, pedido.Estado);
    }

    [Fact]
    public void FlujoCompleto_CambiaLosEstadosEnOrden()
    {
        Pedido pedido = CrearPedidoConProducto();

        pedido.Confirmar();
        pedido.IniciarPreparacion();
        pedido.MarcarComoEnviado();
        pedido.MarcarComoEntregado();

        Assert.Equal(EstadoPedido.Entregado, pedido.Estado);
    }

    [Fact]
    public void MarcarComoEnviado_DesdePendiente_LanzaExcepcion()
    {
        Pedido pedido = CrearPedidoConProducto();

        Action accion = pedido.MarcarComoEnviado;

        Assert.Throws<ExcepcionDominio>(accion);
        Assert.Equal(EstadoPedido.Pendiente, pedido.Estado);
    }

    [Fact]
    public void Cancelar_PedidoConfirmado_CambiaElEstado()
    {
        Pedido pedido = CrearPedidoConProducto();
        pedido.Confirmar();

        pedido.Cancelar();

        Assert.Equal(EstadoPedido.Cancelado, pedido.Estado);
        Assert.NotNull(pedido.FechaCancelacionUtc);
    }

    [Fact]
    public void Cancelar_PedidoEnviado_LanzaExcepcion()
    {
        Pedido pedido = CrearPedidoConProducto();
        pedido.Confirmar();
        pedido.IniciarPreparacion();
        pedido.MarcarComoEnviado();

        Action accion = pedido.Cancelar;

        Assert.Throws<ExcepcionDominio>(accion);
        Assert.Equal(EstadoPedido.Enviado, pedido.Estado);
    }

    [Fact]
    public void AgregarLinea_DespuesDeConfirmar_LanzaExcepcion()
    {
        Pedido pedido = CrearPedidoConProducto();
        pedido.Confirmar();

        Action accion = () => pedido.AgregarLinea(
            Guid.NewGuid(),
            "Monitor",
            250m,
            1);

        Assert.Throws<ExcepcionDominio>(accion);
    }

    private static Pedido CrearPedidoConProducto()
    {
        Pedido pedido = Pedido.Crear(Guid.NewGuid());

        pedido.AgregarLinea(
            Guid.NewGuid(),
            "Teclado mecánico",
            79.99m,
            1);

        return pedido;
    }
}
