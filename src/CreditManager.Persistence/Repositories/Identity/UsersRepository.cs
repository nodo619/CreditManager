using CreditManager.Application.Contracts.Persistence;
using CreditManager.Domain.Entities.Identity;
using CreditManager.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;

namespace CreditManager.Persistence.Repositories.Identity;

public class UsersRepository : BaseRepository<Guid, User>, IUsersRepository
{
    private readonly CreditManagerDbContext _context;

    public UsersRepository(CreditManagerDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username, cancellationToken);
        
        return user;
    }
}