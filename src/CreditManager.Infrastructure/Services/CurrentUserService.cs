using System.Security.Claims;
using CreditManager.Application.Contracts.Infrastructure;
using CreditManager.Application.Contracts.Persistence;
using CreditManager.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;

namespace CreditManager.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUsersRepository _usersRepository;
    private User? _cachedUser;
    
    public CurrentUserService(
        IHttpContextAccessor httpContextAccessor,
        IUsersRepository usersRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _usersRepository = usersRepository;
    }
    
    public async Task<User?> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        if (_cachedUser != null)
            return _cachedUser;

        var username = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(username))
            return null;

        _cachedUser = await _usersRepository.GetByUsernameAsync(username, cancellationToken);
        return _cachedUser;
    }
}