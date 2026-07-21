using Catalog.Application.Productos.Comandos.DesactivarProducto;

namespace Catalog.Application.UnitTests.Productos.Comandos.DesactivarProducto;

public sealed class DesactivarProductoComandoValidadorTests
{
    private readonly DesactivarProductoComandoValidador _validador = new();

    [Fact]
    public async Task Validar_ConIdValido_DebeSerValido()
    {
        var comando = new DesactivarProductoComando(
            Guid.NewGuid());

        var resultado = await _validador.ValidateAsync(comando);

        Assert.True(resultado.IsValid);
    }

    [Fact]
    public async Task Validar_ConIdVacio_DebeSerInvalido()
    {
        var comando = new DesactivarProductoComando(
            Guid.Empty);

        var resultado = await _validador.ValidateAsync(comando);

        Assert.False(resultado.IsValid);
        Assert.Contains(
            resultado.Errors,
            error => error.PropertyName == "Id");
    }
}
