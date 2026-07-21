using MediatR;

namespace Catalog.Application.Productos.Consultas.ObtenerProductos;

public sealed record ObtenerProductosConsulta
    : IRequest<IReadOnlyCollection<ProductoDto>>;
