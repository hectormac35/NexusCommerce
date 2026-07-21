using Catalog.Application.Abstracciones.Persistencia;
using Catalog.Application.Productos;
using Catalog.Application.Productos.Comandos.ActualizarProducto;
using Catalog.Domain.Productos;
using NSubstitute;

namespace Catalog.Application.UnitTests.Productos.Comandos.ActualizarProducto;

public sealed class ActualizarProductoComandoHandlerTests
{
    private readonly IRepositorioProductos _repositorio;
    private readonly ActualizarProductoComandoHandler _handler;

    public ActualizarProductoComandoHandlerTests()
    {
        _repositorio = Substitute.For<IRepositorioProductos>();
        _handler = new ActualizarProductoComandoHandler(_repositorio);
    }

    [Fact]
    public async Task Handle_ConDatosValidos_DebeActualizarProducto()
    {
        var producto = CrearProducto();
        var comando = CrearComando(producto.Id);

        _repositorio
            .ObtenerPorIdAsync(
                producto.Id,
                Arg.Any<CancellationToken>())
            .Returns(producto);

        _repositorio
            .ExisteNombreAsync(
                comando.Nombre,
                producto.Id,
                Arg.Any<CancellationToken>())
            .Returns(false);

        _repositorio
            .GuardarCambiosAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        var resultado = await _handler.Handle(
            comando,
            CancellationToken.None);

        Assert.True(resultado.EsExitoso);
        Assert.Equal(comando.Nombre, producto.Nombre);
        Assert.Equal(comando.Descripcion, producto.Descripcion);
        Assert.Equal(comando.Precio, producto.Precio);
        Assert.Equal(comando.Categoria, producto.Categoria);
        Assert.NotNull(producto.FechaActualizacionUtc);

        await _repositorio.Received(1).GuardarCambiosAsync(
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ConProductoInexistente_DebeDevolverNoEncontrado()
    {
        var id = Guid.NewGuid();
        var comando = CrearComando(id);

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

    [Fact]
    public async Task Handle_ConNombreDuplicado_DebeDevolverConflicto()
    {
        var producto = CrearProducto();
        var comando = CrearComando(producto.Id);

        _repositorio
            .ObtenerPorIdAsync(
                producto.Id,
                Arg.Any<CancellationToken>())
            .Returns(producto);

        _repositorio
            .ExisteNombreAsync(
                comando.Nombre,
                producto.Id,
                Arg.Any<CancellationToken>())
            .Returns(true);

        var resultado = await _handler.Handle(
            comando,
            CancellationToken.None);

        Assert.True(resultado.EsFallo);
        Assert.Equal(
            ErroresProducto.NombreDuplicado,
            resultado.Error);

        await _repositorio.DidNotReceive().GuardarCambiosAsync(
            Arg.Any<CancellationToken>());
    }

    private static Producto CrearProducto()
    {
        return new Producto(
            Guid.NewGuid(),
            "Teclado mecánico",
            "Teclado compacto.",
            89.99m,
            10,
            "Periféricos");
    }

    private static ActualizarProductoComando CrearComando(Guid id)
    {
        return new ActualizarProductoComando(
            id,
            "Teclado profesional",
            "Teclado mecánico profesional.",
            119.99m,
            "Accesorios");
    }
}
