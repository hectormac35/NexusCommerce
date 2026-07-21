using Catalog.Application.Common.Results;
using MediatR;

namespace Catalog.Application.Productos.Comandos.CrearProducto;

public sealed record CrearProductoComando(
    string Nombre,
    string Descripcion,
    decimal Precio,
    int Stock,
    string Categoria)
    : IRequest<Resultado<Guid>>;
