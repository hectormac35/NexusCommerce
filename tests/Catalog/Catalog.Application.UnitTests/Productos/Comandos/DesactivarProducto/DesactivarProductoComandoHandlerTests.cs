using Catalog.Application.Abstracciones.Persistencia;
using Catalog.Application.Productos;
using Catalog.Application.Productos.Comandos.DesactivarProducto;
using Catalog.Domain.Productos;
using NSubstitute;

namespace Catalog.Application.UnitTests.Productos.Comandos.DesactivarProducto;

public sealed class DesactivarProductoComandoHandlerTests
{
    private readonly IRepositorioProductos _repositorio;
    private readonly DesactivarProductoComandoHandler _handler;

    public DesactivarProductoComandoHandlerTests()
    {
        _repositorio = Substitute.For<IRepositorioProductos>();
        _handler = new DesactivarProductoComandoHandler(_repositorio);
    }

    [Fact]
    public async Task Handle_ConProductoExistente_DebeDesactivarlo()
    {
        var producto = CrearProducto();
        var comando = new DesactivarProductoComando(producto.Id);

        _repositorio
            .ObtenerPorIdAsync(
                producto.Id,
                Arg.Any<CancellationToken>())
            .Returns(producto);

        _repositorio
            .GuardarCambiosAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        var resultado = await _handler.Handle(
            comando,
            CancellationToken.None);

        Assert.True(resultado.EsExitoso);
        Assert.False(producto.EstaActivo);
        Assert.NotNull(producto.FechaActualizacionUtc);

        await _repositorio.Received(1).GuardarCambiosAsync(
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ConProductoInexistente_DebeDevolverNoEncontrado()
    {
        var id = Guid.NewGuid();
        var comando = new DesactivarProductoComando(id);

        _repositorio
            .ObtenerPorIdAsync(
                id,
                Arg.Any<CancellationToken>())
            .Returns((Producto?)null);

        var resultado = await _handler.Handle(
            comando,
            CancellationToken.None);

        Assert.True(resultado.EsFallo);
        Assert.Equal(
            ErroresProducto.NoEncontrado(id),
            resultado.Error);

        await _repositorio.DidNotReceive().GuardarCambiosAsync(
            Arg.Any<CancellationToken>());
    }

    private static Producto CrearProducto()
    {
        return new Producto(
            Guid.NewGuid(),
            "Monitor profesional",
            "Monitor IPS profesional.",
            299.99m,
            8,
            "Monitores");
    }
}
