namespace CreditManager.Application.Contracts.Persistence;

public interface IAsyncRepository<in TKey, T>
    where TKey : struct, IEquatable<TKey>
    where T : class
{
    Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken);

    Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken);

    Task<T> AddAsync(T entity, CancellationToken cancellationToken);
    
    Task UpdateAsync(T entity, CancellationToken cancellationToken);
    
    Task DeleteAsync(T entity, CancellationToken cancellationToken);
}