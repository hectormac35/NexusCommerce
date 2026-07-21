using Catalog.Application.Productos.Comandos.ActualizarProducto;

namespace Catalog.Application.UnitTests.Productos.Comandos.ActualizarProducto;

public sealed class ActualizarProductoComandoValidadorTests
{
    private readonly ActualizarProductoComandoValidador _validador = new();

    [Fact]
    public async Task Validar_ConDatosValidos_DebeSerValido()
    {
        var comando = CrearComandoValido();

        var resultado = await _validador.ValidateAsync(comando);

        Assert.True(resultado.IsValid);
        Assert.Empty(resultado.Errors);
    }

    [Fact]
    public async Task Validar_ConDatosInvalidos_DebeDevolverErrores()
    {
        var comando = new ActualizarProductoComando(
            Guid.Empty,
            string.Empty,
            string.Empty,
            0,
            string.Empty);

        var resultado = await _validador.ValidateAsync(comando);

        Assert.False(resultado.IsValid);
        Assert.Contains(
            resultado.Errors,
            error => error.PropertyName == "Id");

        Assert.Contains(
            resultado.Errors,
            error => error.PropertyName == "Nombre");

        Assert.Contains(
            resultado.Errors,
            error => error.PropertyName == "Descripcion");

        Assert.Contains(
            resultado.Errors,
            error => error.PropertyName == "Precio");

        Assert.Contains(
            resultado.Errors,
            error => error.PropertyName == "Categoria");
    }

    private static ActualizarProductoComando CrearComandoValido()
    {
        return new ActualizarProductoComando(
            Guid.NewGuid(),
            "Teclado profesional",
            "Teclado mecánico profesional.",
            119.99m,
            "Accesorios");
    }
}
