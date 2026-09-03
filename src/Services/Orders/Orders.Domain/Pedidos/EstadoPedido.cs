namespace Orders.Domain.Pedidos;

public enum EstadoPedido
{
    Pendiente = 1,
    Confirmado = 2,
    EnPreparacion = 3,
    Enviado = 4,
    Entregado = 5,
    Cancelado = 6
}
