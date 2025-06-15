using CreditManager.Identity.Models;

namespace CreditManager.Identity.Services;

public interface IIdentityService
{
    public Task<LoginResponseModel?> Authenticate(LoginRequestModel request, CancellationToken cancellationToken);

    public Task<RegisterResponseModel> RegisterUser(RegisterRequestModel user, CancellationToken cancellationToken);

}