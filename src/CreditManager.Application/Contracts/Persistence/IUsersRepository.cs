using CreditManager.Domain.Entities.Identity;

namespace CreditManager.Application.Contracts.Persistence;

public interface IUsersRepository : IAsyncRepository<Guid, User>
{
    public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
}