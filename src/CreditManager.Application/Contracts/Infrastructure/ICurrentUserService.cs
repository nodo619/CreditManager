using CreditManager.Domain.Entities.Identity;

namespace CreditManager.Application.Contracts.Infrastructure;

public interface ICurrentUserService
{
    public Task<User?> GetCurrentUserAsync(CancellationToken cancellationToken);
}