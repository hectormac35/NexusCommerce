using Catalog.Application.Common.Results;
using MediatR;

namespace Catalog.Application.Productos.Comandos.DesactivarProducto;

public sealed record DesactivarProductoComando(Guid Id)
    : IRequest<Resultado>;
