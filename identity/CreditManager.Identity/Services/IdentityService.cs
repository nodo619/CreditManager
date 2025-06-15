using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CreditManager.Domain.Entities.Identity;
using CreditManager.Identity.Handlers;
using CreditManager.Identity.Models;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace CreditManager.Identity.Services;

public class IdentityService : IIdentityService
{
    private readonly IConfiguration _configuration;
    private readonly IUsersService _usersService;

    public IdentityService(IConfiguration configuration, IUsersService usersService)
    {
        _configuration = configuration;
        _usersService = usersService;
    }

    public async Task<RegisterResponseModel> RegisterUser(RegisterRequestModel user, CancellationToken cancellationToken)
    {
        var result = await _usersService.AddUser(user, cancellationToken);
        
        return result;
    }
    
    public async Task<LoginResponseModel?> Authenticate(LoginRequestModel request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return null;
        }
        
        var user = await _usersService.FindByUsernameAsync(request.Username, cancellationToken);

        if (user is null || !PasswordHashHandler.VerifyPassword(request.Password, user.PasswordHash))
        {
            return null;
        }
        
        var issuer = _configuration["JwtConfig:Issuer"];
        var audience = _configuration["JwtConfig:Audience"];
        var key = _configuration["JwtConfig:Key"];
        var tokenValidityMinutes = _configuration["JwtConfig:TokenValidityMinutes"];
        var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(double.Parse(tokenValidityMinutes!));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Username),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new Claim("role", ((int)user.Role).ToString())
            }),
            Expires = tokenExpiryTimeStamp,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(securityToken);

        return new LoginResponseModel
        {
            AccessToken = accessToken,
            UserName = user.Username,
            ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds
        };
    }
}