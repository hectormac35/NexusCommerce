using Catalog.Application.Common.Errors;
using Catalog.Application.Common.Results;

namespace Catalog.Domain.UnitTests.Common.Results;

public sealed class ResultadoTests
{
    [Fact]
    public void Exito_ConValor_DebePermitirAccederAlValor()
    {
        var resultado = Resultado<string>.Exito("correcto");

        Assert.True(resultado.EsExitoso);
        Assert.False(resultado.EsFallo);
        Assert.Equal("correcto", resultado.Valor);
        Assert.Equal(Error.Ninguno, resultado.Error);
    }

    [Fact]
    public void Fallo_ConError_DebeCrearResultadoFallido()
    {
        var error = Error.Validacion(
            "Prueba.Error",
            "Se produjo un error de validación.");

        var resultado = Resultado<string>.Fallo(error);

        Assert.False(resultado.EsExitoso);
        Assert.True(resultado.EsFallo);
        Assert.Equal(error, resultado.Error);
    }

    [Fact]
    public void Fallo_AlAccederAlValor_DebeLanzarExcepcion()
    {
        var resultado = Resultado<string>.Fallo(
            Error.Interno(
                "Prueba.ErrorInterno",
                "Error interno."));

        var excepcion = Assert.Throws<InvalidOperationException>(
            () => resultado.Valor);

        Assert.Equal(
            "No se puede acceder al valor de un resultado fallido.",
            excepcion.Message);
    }

    [Fact]
    public void ConversionImplicitaDesdeValor_DebeCrearResultadoExitoso()
    {
        Resultado<int> resultado = 42;

        Assert.True(resultado.EsExitoso);
        Assert.Equal(42, resultado.Valor);
    }

    [Fact]
    public void ConversionImplicitaDesdeError_DebeCrearResultadoFallido()
    {
        var error = Error.NoEncontrado(
            "Prueba.NoEncontrado",
            "No se encontró el recurso.");

        Resultado<int> resultado = error;

        Assert.True(resultado.EsFallo);
        Assert.Equal(error, resultado.Error);
    }
}
