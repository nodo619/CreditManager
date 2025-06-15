using CreditManager.Domain.Entities.Identity;
using CreditManager.Identity.Models;

namespace CreditManager.Identity.Services;

public interface IUsersService
{
    public Task<User?> FindByUsernameAsync(string username, CancellationToken cancellationToken);

    public Task<RegisterResponseModel> AddUser(RegisterRequestModel userDto, CancellationToken cancellationToken);

}