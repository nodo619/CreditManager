using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CreditManager.Identity.Data;

public class CreditManagerIdentityDbContext : IdentityDbContext<CreditManagerUser, IdentityRole<Guid>, Guid>
{
    public CreditManagerIdentityDbContext(DbContextOptions<CreditManagerIdentityDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("identity");

        builder.Entity<CreditManagerUser>(entity =>
        {
            entity.ToTable(name: "Users");
        });
        builder.Entity<IdentityRole<Guid>>(entity =>
        {
            entity.ToTable(name: "Roles");
        });
        builder.Entity<IdentityUserRole<Guid>>(entity =>
        {
            entity.ToTable(name: "UserRoles");
        });
        builder.Entity<IdentityUserClaim<Guid>>(entity =>
        {
            entity.ToTable(name: "UserClaims");
        });
        builder.Entity<IdentityUserLogin<Guid>>(entity =>
        {
            entity.ToTable(name: "UserLogins");
        });
        builder.Entity<IdentityRoleClaim<Guid>>(entity =>
        {
            entity.ToTable(name: "RoleClaims");
        });
        builder.Entity<IdentityUserToken<Guid>>(entity =>
        {
            entity.ToTable(name: "UserTokens");
        });

        builder.SeedUsers();
        builder.SeedRoles();
    }
}