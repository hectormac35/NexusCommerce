namespace Orders.Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    Task<int> GuardarCambiosAsync(
        CancellationToken cancellationToken = default);
}
