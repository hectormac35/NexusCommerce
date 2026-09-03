namespace NexusCommerce.Gateway.RabbitMq.Services;

public interface IRabbitMqMonitoringService
{
    Task<RabbitMqResponse> GetOverviewAsync();
}
