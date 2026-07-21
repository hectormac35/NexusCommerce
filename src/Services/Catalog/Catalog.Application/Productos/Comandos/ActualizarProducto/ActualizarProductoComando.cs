using Catalog.Application.Common.Results;
using MediatR;

namespace Catalog.Application.Productos.Comandos.ActualizarProducto;

public sealed record ActualizarProductoComando(
    Guid Id,
    string Nombre,
    string Descripcion,
    decimal Precio,
    string Categoria)
    : IRequest<Resultado>;
