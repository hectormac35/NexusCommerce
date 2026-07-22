using Catalog.Application.Abstracciones.Mensajeria;

namespace Catalog.Application.Eventos.Integracion;

public sealed record ProductoCreadoEvento(
    Guid EventoId,
    DateTime OcurridoEnUtc,
    Guid ProductoId,
    string Nombre,
    decimal Precio,
    int Stock,
    string Categoria)
    : IEventoIntegracion;
