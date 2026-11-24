using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Abstractions.Catalogo;
using Orders.Application.Abstractions.Persistence;
using Orders.Infrastructure.Catalogo;
using Orders.Infrastructure.Persistence;
using Orders.Infrastructure.Persistence.Repositories;

namespace Orders.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddOrdersInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string connectionString =
            configuration.GetConnectionString("OrdersDatabase")
            ?? throw new InvalidOperationException(
                "No se ha configurado la conexión 'OrdersDatabase'.");

        services.AddDbContext<OrdersDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IPedidoRepository, PedidoRepository>();

        services.AddScoped<IUnitOfWork>(
            serviceProvider =>
                serviceProvider.GetRequiredService<OrdersDbContext>());

        string catalogoUrl =
            configuration["Servicios:CatalogoUrl"]
            ?? throw new InvalidOperationException(
                "No se ha configurado 'Servicios:CatalogoUrl'.");

        services.AddHttpClient<ICatalogoProductos, CatalogoHttpCliente>(cliente =>
        {
            cliente.BaseAddress = new Uri(catalogoUrl);
            cliente.Timeout = TimeSpan.FromSeconds(5);
        });

        return services;
    }
}
