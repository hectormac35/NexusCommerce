using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Orders.Application.Abstractions.Catalogo;
using Orders.Application.Abstractions.Persistence;
using Orders.Application.Pedidos.Commands.CreateOrder;
using Orders.Domain.Pedidos;

namespace Orders.Application.UnitTests.Pedidos;

public sealed class CreateOrderHandlerTests
{
    private static readonly Guid ClienteId = Guid.NewGuid();

    private readonly IPedidoRepository _repositorio = Substitute.For<IPedidoRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ICatalogoProductos _catalogo = Substitute.For<ICatalogoProductos>();
    private readonly CreateOrderHandler _handler;

    public CreateOrderHandlerTests()
    {
        _handler = new CreateOrderHandler(_repositorio, _unitOfWork, _catalogo);
    }

    private ProductoCatalogo EnCatalogo(
        decimal precio = 25m,
        int stock = 10,
        bool activo = true,
        string nombre = "Teclado mecánico")
    {
        var producto = new ProductoCatalogo(Guid.NewGuid(), nombre, precio, stock, activo);

        _catalogo
            .ObtenerAsync(producto.Id, Arg.Any<CancellationToken>())
            .Returns(producto);

        return producto;
    }

    private Pedido? PedidoGuardado()
    {
        return _repositorio.ReceivedCalls()
            .Select(llamada => llamada.GetArguments().FirstOrDefault())
            .OfType<Pedido>()
            .SingleOrDefault();
    }

    [Fact]
    public async Task Handle_UsaElPrecioYElNombreDelCatalogo()
    {
        var producto = EnCatalogo(precio: 49.90m, nombre: "Monitor 27\"");

        var resultado = await _handler.Handle(
            new CreateOrderCommand(ClienteId, [new(producto.Id, 2)]),
            CancellationToken.None);

        Assert.True(resultado.Success);
        Assert.Equal(99.80m, resultado.Value!.Total);

        var linea = Assert.Single(PedidoGuardado()!.Lineas);
        Assert.Equal("Monitor 27\"", linea.NombreProducto);
        Assert.Equal(49.90m, linea.PrecioUnitario);
        await _unitOfWork.Received(1).GuardarCambiosAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_AgrupaLineasDelMismoProducto()
    {
        var producto = EnCatalogo(precio: 10m);

        var resultado = await _handler.Handle(
            new CreateOrderCommand(ClienteId, [new(producto.Id, 1), new(producto.Id, 3)]),
            CancellationToken.None);

        Assert.True(resultado.Success);
        var linea = Assert.Single(PedidoGuardado()!.Lineas);
        Assert.Equal(4, linea.Cantidad);
        Assert.Equal(40m, resultado.Value!.Total);
    }

    [Fact]
    public async Task Handle_ProductoInexistente_Falla()
    {
        var resultado = await _handler.Handle(
            new CreateOrderCommand(ClienteId, [new(Guid.NewGuid(), 1)]),
            CancellationToken.None);

        Assert.True(resultado.Failure);
        Assert.Contains("no existe", resultado.Error);
        await _unitOfWork.DidNotReceive().GuardarCambiosAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ProductoInactivo_Falla()
    {
        var producto = EnCatalogo(activo: false);

        var resultado = await _handler.Handle(
            new CreateOrderCommand(ClienteId, [new(producto.Id, 1)]),
            CancellationToken.None);

        Assert.True(resultado.Failure);
        Assert.Contains("no está disponible", resultado.Error);
    }

    [Fact]
    public async Task Handle_SinStockSuficiente_Falla()
    {
        var producto = EnCatalogo(stock: 2);

        var resultado = await _handler.Handle(
            new CreateOrderCommand(ClienteId, [new(producto.Id, 3)]),
            CancellationToken.None);

        Assert.True(resultado.Failure);
        Assert.Contains("stock", resultado.Error);
        await _unitOfWork.DidNotReceive().GuardarCambiosAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_CatalogoCaido_DevuelveErrorControlado()
    {
        _catalogo
            .ObtenerAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new CatalogoNoDisponibleException("caído"));

        var resultado = await _handler.Handle(
            new CreateOrderCommand(ClienteId, [new(Guid.NewGuid(), 1)]),
            CancellationToken.None);

        Assert.True(resultado.Failure);
        Assert.Contains("catálogo no está disponible", resultado.Error);
    }
}
