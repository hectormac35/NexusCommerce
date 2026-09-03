using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Pedidos.Commands.CreateOrder;
using Orders.Application.Pedidos.Commands.ChangeOrderStatus;

namespace Orders.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddOrdersApplication(
        this IServiceCollection services)
    {
        services.AddMediatR(configuracion =>
            configuracion.RegisterServicesFromAssembly(
                typeof(DependencyInjection).Assembly));

        services.AddScoped<
            IValidator<CreateOrderCommand>,
            CreateOrderCommandValidator>();

        services.AddScoped<
            IValidator<ChangeOrderStatusCommand>,
            ChangeOrderStatusCommandValidator>();

        return services;
    }
}
