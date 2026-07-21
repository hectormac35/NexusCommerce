using Catalog.Domain.Excepciones;
using Catalog.Domain.Productos;

namespace Catalog.Domain.UnitTests.Productos;

public sealed class ProductoTests
{
    [Fact]
    public void CrearProducto_ConDatosValidos_DebeCrearProductoActivo()
    {
        var producto = CrearProducto();

        Assert.NotEqual(Guid.Empty, producto.Id);
        Assert.Equal("Teclado mecánico", producto.Nombre);
        Assert.Equal(89.99m, producto.Precio);
        Assert.Equal(10, producto.Stock);
        Assert.True(producto.EstaActivo);
        Assert.True(producto.TieneStock);
        Assert.Null(producto.FechaActualizacionUtc);
    }

    [Fact]
    public void CrearProducto_ConNombreVacio_DebeLanzarExcepcionDominio()
    {
        var excepcion = Assert.Throws<ExcepcionDominio>(() =>
            new Producto(
                Guid.NewGuid(),
                " ",
                "Descripción válida",
                50m,
                5,
                "Periféricos"));

        Assert.Equal(
            "El nombre del producto es obligatorio.",
            excepcion.Message);
    }

    [Fact]
    public void CrearProducto_ConPrecioCero_DebeLanzarExcepcionDominio()
    {
        var excepcion = Assert.Throws<ExcepcionDominio>(() =>
            new Producto(
                Guid.NewGuid(),
                "Producto",
                "Descripción válida",
                0m,
                5,
                "Periféricos"));

        Assert.Equal(
            "El precio del producto debe ser mayor que cero.",
            excepcion.Message);
    }

    [Fact]
    public void CrearProducto_ConStockNegativo_DebeLanzarExcepcionDominio()
    {
        var excepcion = Assert.Throws<ExcepcionDominio>(() =>
            new Producto(
                Guid.NewGuid(),
                "Producto",
                "Descripción válida",
                10m,
                -1,
                "Periféricos"));

        Assert.Equal(
            "El stock no puede ser negativo.",
            excepcion.Message);
    }

    [Fact]
    public void IncrementarStock_ConCantidadValida_DebeAumentarStock()
    {
        var producto = CrearProducto();

        producto.IncrementarStock(5);

        Assert.Equal(15, producto.Stock);
        Assert.NotNull(producto.FechaActualizacionUtc);
    }

    [Fact]
    public void ReducirStock_ConCantidadValida_DebeReducirStock()
    {
        var producto = CrearProducto();

        producto.ReducirStock(4);

        Assert.Equal(6, producto.Stock);
        Assert.True(producto.TieneStock);
        Assert.NotNull(producto.FechaActualizacionUtc);
    }

    [Fact]
    public void ReducirStock_HastaCero_DebeIndicarQueNoTieneStock()
    {
        var producto = CrearProducto();

        producto.ReducirStock(10);

        Assert.Equal(0, producto.Stock);
        Assert.False(producto.TieneStock);
    }

    [Fact]
    public void ReducirStock_PorEncimaDelDisponible_DebeLanzarExcepcionDominio()
    {
        var producto = CrearProducto();

        var excepcion = Assert.Throws<ExcepcionDominio>(() =>
            producto.ReducirStock(11));

        Assert.Equal(
            "No existe stock suficiente. Disponible: 10.",
            excepcion.Message);
    }

    [Fact]
    public void Desactivar_DebeCambiarEstadoAInactivo()
    {
        var producto = CrearProducto();

        producto.Desactivar();

        Assert.False(producto.EstaActivo);
        Assert.NotNull(producto.FechaActualizacionUtc);
    }

    private static Producto CrearProducto()
    {
        return new Producto(
            Guid.NewGuid(),
            "Teclado mecánico",
            "Teclado mecánico compacto.",
            89.99m,
            10,
            "Periféricos");
    }
}
