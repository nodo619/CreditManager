using CreditManager.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace CreditManager.Identity.Handlers;

public static class PasswordHashHandler
{
    public static string HashPassword(string password)
    {
        var hasher = new PasswordHasher<User>();
        
        return hasher.HashPassword(null, password);
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        var hasher = new PasswordHasher<User>();
        
        var result = hasher.VerifyHashedPassword(null, hashedPassword, password);
        
        return result is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
    }
}