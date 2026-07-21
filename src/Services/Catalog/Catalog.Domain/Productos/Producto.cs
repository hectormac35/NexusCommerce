using Catalog.Domain.Excepciones;

namespace Catalog.Domain.Productos;

public sealed class Producto
{
    private const int LongitudMaximaNombre = 150;
    private const int LongitudMaximaDescripcion = 1_000;
    private const int LongitudMaximaCategoria = 100;

    private Producto()
    {
    }

    public Producto(
        Guid id,
        string nombre,
        string descripcion,
        decimal precio,
        int stock,
        string categoria)
    {
        if (id == Guid.Empty)
        {
            throw new ExcepcionDominio(
                "El identificador del producto no puede estar vacío.");
        }

        Id = id;
        CambiarNombre(nombre);
        CambiarDescripcion(descripcion);
        CambiarPrecio(precio);
        CambiarCategoria(categoria);
        EstablecerStock(stock);

        EstaActivo = true;
        FechaCreacionUtc = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public string Nombre { get; private set; } = string.Empty;

    public string Descripcion { get; private set; } = string.Empty;

    public decimal Precio { get; private set; }

    public int Stock { get; private set; }

    public string Categoria { get; private set; } = string.Empty;

    public bool EstaActivo { get; private set; }

    public DateTime FechaCreacionUtc { get; private set; }

    public DateTime? FechaActualizacionUtc { get; private set; }

    public bool TieneStock => Stock > 0;

    public void ActualizarInformacion(
        string nombre,
        string descripcion,
        decimal precio,
        string categoria)
    {
        CambiarNombre(nombre);
        CambiarDescripcion(descripcion);
        CambiarPrecio(precio);
        CambiarCategoria(categoria);
        MarcarComoActualizado();
    }

    public void IncrementarStock(int cantidad)
    {
        ValidarCantidadPositiva(cantidad);

        checked
        {
            Stock += cantidad;
        }

        MarcarComoActualizado();
    }

    public void ReducirStock(int cantidad)
    {
        ValidarCantidadPositiva(cantidad);

        if (cantidad > Stock)
        {
            throw new ExcepcionDominio(
                $"No existe stock suficiente. Disponible: {Stock}.");
        }

        Stock -= cantidad;
        MarcarComoActualizado();
    }

    public void Activar()
    {
        EstaActivo = true;
        MarcarComoActualizado();
    }

    public void Desactivar()
    {
        EstaActivo = false;
        MarcarComoActualizado();
    }

    private void CambiarNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ExcepcionDominio(
                "El nombre del producto es obligatorio.");
        }

        nombre = nombre.Trim();

        if (nombre.Length > LongitudMaximaNombre)
        {
            throw new ExcepcionDominio(
                $"El nombre no puede superar los {LongitudMaximaNombre} caracteres.");
        }

        Nombre = nombre;
    }

    private void CambiarDescripcion(string descripcion)
    {
        if (string.IsNullOrWhiteSpace(descripcion))
        {
            throw new ExcepcionDominio(
                "La descripción del producto es obligatoria.");
        }

        descripcion = descripcion.Trim();

        if (descripcion.Length > LongitudMaximaDescripcion)
        {
            throw new ExcepcionDominio(
                $"La descripción no puede superar los {LongitudMaximaDescripcion} caracteres.");
        }

        Descripcion = descripcion;
    }

    private void CambiarPrecio(decimal precio)
    {
        if (precio <= 0)
        {
            throw new ExcepcionDominio(
                "El precio del producto debe ser mayor que cero.");
        }

        Precio = decimal.Round(
            precio,
            2,
            MidpointRounding.AwayFromZero);
    }

    private void EstablecerStock(int stock)
    {
        if (stock < 0)
        {
            throw new ExcepcionDominio(
                "El stock no puede ser negativo.");
        }

        Stock = stock;
    }

    private void CambiarCategoria(string categoria)
    {
        if (string.IsNullOrWhiteSpace(categoria))
        {
            throw new ExcepcionDominio(
                "La categoría del producto es obligatoria.");
        }

        categoria = categoria.Trim();

        if (categoria.Length > LongitudMaximaCategoria)
        {
            throw new ExcepcionDominio(
                $"La categoría no puede superar los {LongitudMaximaCategoria} caracteres.");
        }

        Categoria = categoria;
    }

    private static void ValidarCantidadPositiva(int cantidad)
    {
        if (cantidad <= 0)
        {
            throw new ExcepcionDominio(
                "La cantidad debe ser mayor que cero.");
        }
    }

    private void MarcarComoActualizado()
    {
        FechaActualizacionUtc = DateTime.UtcNow;
    }
}
