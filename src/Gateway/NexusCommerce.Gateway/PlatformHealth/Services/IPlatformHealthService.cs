namespace NexusCommerce.Gateway.PlatformHealth.Services;

public interface IPlatformHealthService
{
    Task<PlatformHealthResponse> GetHealthAsync();
}
