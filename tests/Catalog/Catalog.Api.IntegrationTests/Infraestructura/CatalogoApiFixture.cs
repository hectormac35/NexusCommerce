using Testcontainers.PostgreSql;

namespace Catalog.Api.IntegrationTests.Infraestructura;

public sealed class CatalogoApiFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres =
        new PostgreSqlBuilder("postgres:18-alpine")
            .WithDatabase("nexus_catalogo_tests")
            .WithUsername("nexus_tests")
            .WithPassword("nexus_tests_password")
            .Build();

    private CatalogoApiFactory? _factory;

    public HttpClient Cliente { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        _factory = new CatalogoApiFactory(
            _postgres.GetConnectionString());

        Cliente = _factory.CreateClient();
    }

    public async Task DisposeAsync()
    {
        Cliente.Dispose();

        if (_factory is not null)
        {
            await _factory.DisposeAsync();
        }

        await _postgres.DisposeAsync();
    }
}
