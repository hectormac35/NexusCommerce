using Orders.Application.Pedidos.Commands.CreateOrder;

namespace Orders.Application.UnitTests.Pedidos;

public sealed class CreateOrderCommandValidatorTests
{
    private readonly CreateOrderCommandValidator _validador = new();

    [Fact]
    public void Valida_PedidoCorrecto()
    {
        var comando = new CreateOrderCommand(Guid.NewGuid(), [new(Guid.NewGuid(), 2)]);

        Assert.True(_validador.Validate(comando).IsValid);
    }

    [Fact]
    public void Rechaza_PedidoSinLineas()
    {
        var comando = new CreateOrderCommand(Guid.NewGuid(), []);

        Assert.False(_validador.Validate(comando).IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(CreateOrderCommandValidator.MaximoUnidadesPorLinea + 1)]
    public void Rechaza_CantidadesFueraDeRango(int cantidad)
    {
        var comando = new CreateOrderCommand(Guid.NewGuid(), [new(Guid.NewGuid(), cantidad)]);

        Assert.False(_validador.Validate(comando).IsValid);
    }

    [Fact]
    public void Rechaza_DemasiadasLineas()
    {
        var lineas = Enumerable
            .Range(0, CreateOrderCommandValidator.MaximoLineas + 1)
            .Select(_ => new CreateOrderItemRequest(Guid.NewGuid(), 1))
            .ToList();

        Assert.False(_validador.Validate(new CreateOrderCommand(Guid.NewGuid(), lineas)).IsValid);
    }
}
