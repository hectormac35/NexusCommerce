using Catalog.Application.Productos.Comandos.CrearProducto;

namespace Catalog.Application.UnitTests.Productos.Comandos.CrearProducto;

public sealed class CrearProductoComandoValidadorTests
{
    private readonly CrearProductoComandoValidador _validador = new();

    [Fact]
    public async Task Validar_ConDatosValidos_DebeSerValido()
    {
        var comando = new CrearProductoComando(
            "Webcam profesional",
            "Webcam Full HD para videoconferencias.",
            74.95m,
            15,
            "Periféricos");

        var resultado = await _validador.ValidateAsync(comando);

        Assert.True(resultado.IsValid);
        Assert.Empty(resultado.Errors);
    }

    [Fact]
    public async Task Validar_ConDatosInvalidos_DebeDevolverTodosLosErrores()
    {
        var comando = new CrearProductoComando(
            string.Empty,
            string.Empty,
            0,
            -1,
            string.Empty);

        var resultado = await _validador.ValidateAsync(comando);

        Assert.False(resultado.IsValid);
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
            error => error.PropertyName == "Stock");

        Assert.Contains(
            resultado.Errors,
            error => error.PropertyName == "Categoria");
    }

    [Fact]
    public async Task Validar_ConNombreDemasiadoLargo_DebeSerInvalido()
    {
        var comando = new CrearProductoComando(
            new string('A', 151),
            "Descripción válida",
            10m,
            1,
            "Categoría");

        var resultado = await _validador.ValidateAsync(comando);

        Assert.Contains(
            resultado.Errors,
            error =>
                error.PropertyName == "Nombre" &&
                error.ErrorMessage ==
                "El nombre no puede superar los 150 caracteres.");
    }
}
