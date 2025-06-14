using Duende.IdentityServer.Models;
using System.Security.Claims;

namespace CreditManager.Identity;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResource("roles", "User roles", new[] { "role" })
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("creditmanager.api", "Credit Manager API"),
            new ApiScope("creditmanager.api.read", "Credit Manager API - Read Only"),
            new ApiScope("creditmanager.api.write", "Credit Manager API - Write Access")
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // Web Client
            new Client
            {
                ClientId = "creditmanager.web",
                ClientName = "Credit Manager Web Client",
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RequireClientSecret = false,
                RedirectUris = { "https://localhost:7002/signin-oidc" },
                PostLogoutRedirectUris = { "https://localhost:7002/signout-callback-oidc" },
                AllowedScopes = 
                {
                    "openid",
                    "profile",
                    "email",
                    "roles",
                    "creditmanager.api"
                }
            },
            // API Client
            new Client
            {
                ClientId = "creditmanager.api",
                ClientName = "Credit Manager API",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedScopes = 
                {
                    "creditmanager.api.read",
                    "creditmanager.api.write"
                }
            }
        };
}
