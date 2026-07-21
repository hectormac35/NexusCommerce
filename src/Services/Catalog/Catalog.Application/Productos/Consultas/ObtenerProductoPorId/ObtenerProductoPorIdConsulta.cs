using MediatR;

namespace Catalog.Application.Productos.Consultas.ObtenerProductoPorId;

public sealed record ObtenerProductoPorIdConsulta(Guid Id)
    : IRequest<ProductoDto?>;
