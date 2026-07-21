namespace Catalog.Api.IntegrationTests.Infraestructura;

[CollectionDefinition(Nombre)]
public sealed class ColeccionCatalogoApi
    : ICollectionFixture<CatalogoApiFixture>
{
    public const string Nombre =
        "Coleccion de integración de Catalog API";
}
