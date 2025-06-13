using CreditManager.Application.Contracts.Persistence;
using CreditManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CreditManager.Persistence.Repositories;

public class BaseRepository<TKey, T> : IAsyncRepository<TKey, T>
    where TKey : struct, IEquatable<TKey>, IFormattable
    where T : AuditableEntity<TKey>
{
    private readonly CreditManagerDbContext _dbContext;

    public BaseRepository(CreditManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<T>().FirstOrDefaultAsync(entity => entity.Id.Equals(id), cancellationToken);
    }

    public async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Set<T>().ToListAsync(cancellationToken);
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
    {
        await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken)
    {
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}