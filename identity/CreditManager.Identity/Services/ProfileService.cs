using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using CreditManager.Identity.Data;

namespace CreditManager.Identity.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<CreditManagerUser> _userManager;

        public ProfileService(UserManager<CreditManagerUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);
            if (user == null)
            {
                return;
            }

            var claims = new List<Claim>
            {
                new Claim("first_name", user.FirstName ?? string.Empty),
                new Claim("last_name", user.LastName ?? string.Empty),
                new Claim("name", $"{user.FirstName} {user.LastName}".Trim()),
                new Claim("email", user.Email ?? string.Empty)
            };

            // Add roles
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }

            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);
            context.IsActive = user != null;
        }
    }
} 