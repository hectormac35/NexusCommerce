namespace NexusCommerce.Gateway.Jaeger.Services;

public interface IJaegerMonitoringService
{
    Task<JaegerResponse> GetOverviewAsync();
}
