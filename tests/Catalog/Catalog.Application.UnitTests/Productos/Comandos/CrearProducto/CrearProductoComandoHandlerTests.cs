using Catalog.Application.Abstracciones.Mensajeria;
using Catalog.Application.Abstracciones.Persistencia;
using Catalog.Application.Eventos.Integracion;
using Catalog.Application.Productos;
using Catalog.Application.Productos.Comandos.CrearProducto;
using Catalog.Domain.Productos;
using NSubstitute;

namespace Catalog.Application.UnitTests.Productos.Comandos.CrearProducto;

public sealed class CrearProductoComandoHandlerTests
{
    private readonly IRepositorioProductos _repositorio;
    private readonly IBusEventos _busEventos;
    private readonly CrearProductoComandoHandler _handler;

    public CrearProductoComandoHandlerTests()
    {
        _repositorio = Substitute.For<IRepositorioProductos>();
        _busEventos = Substitute.For<IBusEventos>();

        _handler = new CrearProductoComandoHandler(
            _repositorio,
            _busEventos);
    }

    [Fact]
    public async Task Handle_ConDatosValidos_DebeCrearProducto()
    {
        var comando = CrearComandoValido();

        _repositorio
            .ExisteNombreAsync(
                comando.Nombre,
                null,
                Arg.Any<CancellationToken>())
            .Returns(false);

        _repositorio
            .GuardarCambiosAsync(
                Arg.Any<CancellationToken>())
            .Returns(1);

        var resultado = await _handler.Handle(
            comando,
            CancellationToken.None);

        Assert.True(resultado.EsExitoso);
        Assert.NotEqual(Guid.Empty, resultado.Valor);

        await _repositorio.Received(1).AgregarAsync(
            Arg.Is<Producto>(producto =>
                producto != null &&
                producto.Id == resultado.Valor &&
                producto.Nombre == comando.Nombre &&
                producto.Precio == comando.Precio &&
                producto.Stock == comando.Stock),
            Arg.Any<CancellationToken>());

        await _repositorio.Received(1)
            .GuardarCambiosAsync(
                Arg.Any<CancellationToken>());

        await _busEventos.Received(1)
            .PublicarAsync(
                Arg.Any<ProductoCreadoEvento>(),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ConNombreDuplicado_DebeDevolverConflicto()
    {
        var comando = CrearComandoValido();

        _repositorio
            .ExisteNombreAsync(
                comando.Nombre,
                null,
                Arg.Any<CancellationToken>())
            .Returns(true);

        var resultado = await _handler.Handle(
            comando,
            CancellationToken.None);

        Assert.True(resultado.EsFallo);

        Assert.Equal(
            ErroresProducto.NombreDuplicado,
            resultado.Error);

        await _repositorio.DidNotReceive()
            .AgregarAsync(
                Arg.Any<Producto>(),
                Arg.Any<CancellationToken>());

        await _repositorio.DidNotReceive()
            .GuardarCambiosAsync(
                Arg.Any<CancellationToken>());

        await _busEventos.DidNotReceive()
            .PublicarAsync(
                Arg.Any<ProductoCreadoEvento>(),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_DebeComprobarNombreSinEspaciosLaterales()
    {
        var comando = CrearComandoValido() with
        {
            Nombre = "  Webcam profesional  "
        };

        _repositorio
            .ExisteNombreAsync(
                "Webcam profesional",
                null,
                Arg.Any<CancellationToken>())
            .Returns(false);

        _repositorio
            .GuardarCambiosAsync(
                Arg.Any<CancellationToken>())
            .Returns(1);

        var resultado = await _handler.Handle(
            comando,
            CancellationToken.None);

        Assert.True(resultado.EsExitoso);

        await _repositorio.Received(1)
            .ExisteNombreAsync(
                "Webcam profesional",
                null,
                Arg.Any<CancellationToken>());

        await _repositorio.Received(1)
            .AgregarAsync(
                Arg.Is<Producto>(producto =>
                    producto != null &&
                    producto.Nombre == "Webcam profesional"),
                Arg.Any<CancellationToken>());

        await _busEventos.Received(1)
            .PublicarAsync(
                Arg.Any<ProductoCreadoEvento>(),
                Arg.Any<CancellationToken>());
    }

    private static CrearProductoComando CrearComandoValido()
    {
        return new CrearProductoComando(
            "Webcam profesional",
            "Webcam Full HD para videoconferencias.",
            74.95m,
            15,
            "Periféricos");
    }
}
