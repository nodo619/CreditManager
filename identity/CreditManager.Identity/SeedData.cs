using Microsoft.EntityFrameworkCore;
using Serilog;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;

namespace CreditManager.Identity;

public class SeedData
{
    public static void EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

            var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            context.Database.Migrate();
            EnsureSeedData(context);
        }
    }

    private static void EnsureSeedData(ConfigurationDbContext context)
    {
        // Always update clients
        Log.Debug("Updating clients");
        var existingClients = context.Clients
            .Include(c => c.AllowedGrantTypes)
            .Include(c => c.RedirectUris)
            .Include(c => c.PostLogoutRedirectUris)
            .Include(c => c.AllowedScopes)
            .Include(c => c.AllowedCorsOrigins)
            .ToList();
        var configuredClients = Config.Clients.ToList();

        // Log existing clients
        foreach (var client in existingClients)
        {
            Log.Debug("Removing existing client: {ClientId}", client.ClientId);
            Log.Debug("  AllowedGrantTypes: {GrantTypes}", string.Join(", ", client.AllowedGrantTypes.Select(g => g.GrantType)));
            Log.Debug("  RedirectUris: {RedirectUris}", string.Join(", ", client.RedirectUris.Select(r => r.RedirectUri)));
            Log.Debug("  AllowedScopes: {Scopes}", string.Join(", ", client.AllowedScopes.Select(s => s.Scope)));
            Log.Debug("  AllowedCorsOrigins: {CorsOrigins}", string.Join(", ", client.AllowedCorsOrigins.Select(c => c.Origin)));
            context.Clients.Remove(client);
        }
        context.SaveChanges();

        // Add all configured clients
        foreach (var client in configuredClients)
        {
            Log.Debug("Adding configured client: {ClientId}", client.ClientId);
            Log.Debug("  AllowedGrantTypes: {GrantTypes}", string.Join(", ", client.AllowedGrantTypes));
            Log.Debug("  RedirectUris: {RedirectUris}", string.Join(", ", client.RedirectUris));
            Log.Debug("  AllowedScopes: {Scopes}", string.Join(", ", client.AllowedScopes));
            Log.Debug("  AllowedCorsOrigins: {CorsOrigins}", string.Join(", ", client.AllowedCorsOrigins));
            context.Clients.Add(client.ToEntity());
        }
        context.SaveChanges();

        // Always update identity resources
        Log.Debug("Updating identity resources");
        var existingResources = context.IdentityResources
            .Include(r => r.UserClaims)
            .ToList();
        var configuredResources = Config.IdentityResources.ToList();

        // Remove all existing identity resources
        foreach (var resource in existingResources)
        {
            context.IdentityResources.Remove(resource);
        }
        context.SaveChanges();

        // Add all configured identity resources
        foreach (var resource in configuredResources)
        {
            context.IdentityResources.Add(resource.ToEntity());
        }
        context.SaveChanges();

        // Always update API scopes
        Log.Debug("Updating API scopes");
        var existingScopes = context.ApiScopes
            .Include(s => s.UserClaims)
            .ToList();
        var configuredScopes = Config.ApiScopes.ToList();

        // Remove all existing API scopes
        foreach (var scope in existingScopes)
        {
            context.ApiScopes.Remove(scope);
        }
        context.SaveChanges();

        // Add all configured API scopes
        foreach (var scope in configuredScopes)
        {
            context.ApiScopes.Add(scope.ToEntity());
        }
        context.SaveChanges();

        if (!context.IdentityProviders.Any())
        {
            Log.Debug("OIDC IdentityProviders being populated");
            context.IdentityProviders.Add(new OidcProvider
            {
                Scheme = "demoidsrv",
                DisplayName = "IdentityServer",
                Authority = "https://demo.duendesoftware.com",
                ClientId = "login",
            }.ToEntity());
            context.SaveChanges();
        }
        else
        {
            Log.Debug("OIDC IdentityProviders already populated");
        }
    }
}
