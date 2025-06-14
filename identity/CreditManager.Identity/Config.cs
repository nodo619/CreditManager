using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace CreditManager.Identity;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResource("roles", "User roles", new List<string> { "role" }),
            new IdentityResource("userinfo", "User information", new List<string> { 
                "FirstName",
                "LastName"
            })
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("creditmanager.api", "CreditManager API"),
            new ApiScope("creditmanager.api.read", "CreditManager API Read Access"),
            new ApiScope("creditmanager.api.write", "CreditManager API Write Access")
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            // ExtJS Web Client
            new Client
            {
                ClientId = "creditmanager.ui",
                ClientName = "CreditManager UI",
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RequireClientSecret = false,
                RedirectUris = { "http://localhost:1841/signin-oidc.html" },
                PostLogoutRedirectUris = { "http://localhost:1841/signout-callback-oidc.html" },
                AllowedCorsOrigins = { "http://localhost:1841" },
                AllowedScopes = { 
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "roles",
                    "userinfo",
                    "creditmanager.api",
                    "offline_access"
                },
                AllowOfflineAccess = true,
                AccessTokenLifetime = 3600, // 1 hour
                RefreshTokenUsage = TokenUsage.ReUse,
                RefreshTokenExpiration = TokenExpiration.Sliding,
                SlidingRefreshTokenLifetime = 2592000, // 30 days
                RequireConsent = false,
                AlwaysIncludeUserClaimsInIdToken = true,
                AlwaysSendClientClaims = true,
                Claims = new List<ClientClaim>
                {
                    new ClientClaim("role", "user")
                }
            },
            // API Client
            new Client
            {
                ClientId = "creditmanager.api",
                ClientName = "CreditManager API",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedScopes = { "creditmanager.api.read", "creditmanager.api.write" }
            }
        };
}
