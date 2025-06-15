using CreditManager.Application.Contracts.Persistence;
using CreditManager.Domain.Entities.Identity;
using CreditManager.Domain.Enumerations;
using CreditManager.Identity.Handlers;
using CreditManager.Identity.Models;

namespace CreditManager.Identity.Services;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;

    public UsersService(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<User?> FindByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        return await _usersRepository.GetByUsernameAsync(username, cancellationToken);
    }

    public async Task<RegisterResponseModel> AddUser(RegisterRequestModel userDto, CancellationToken cancellationToken)
    {
        var existingUser = await _usersRepository.GetByUsernameAsync(userDto.Username, cancellationToken);
        
        if (existingUser is not null)
        {
            return new RegisterResponseModel
            {
                FailureMessage = "Username already exists",
            };
        }
        
        var hashedPassword = PasswordHashHandler.HashPassword(userDto.Password);
        
        var userEntity = new User
        {
            Id = Guid.NewGuid(),
            Username = userDto.Username,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            BirthDate = userDto.BirthDate,
            Role = UserRole.Customer,
            PersonalNumber = userDto.PersonalNumber,
            PasswordHash = hashedPassword,

        };

        await _usersRepository.AddAsync(userEntity, cancellationToken);

        return new RegisterResponseModel
        {
            UserId = userEntity.Id
        };
    }
}