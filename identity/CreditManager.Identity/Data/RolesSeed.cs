using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CreditManager.Identity.Data;

public static class RolesSeed
{
    public static void SeedRoles(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityRole<Guid>>().HasData(
            new IdentityRole<Guid>
            {
                Id = Guid.Parse("157e730f-df20-4c5d-be84-22dd80c46ef1"),
                Name = "Customer",
                NormalizedName = "CUSTOMER"
            },
            new IdentityRole<Guid>
            {
                Id = Guid.Parse("157e730f-df20-4c5d-be84-22dd80c46ef2"),
                Name = "CreditOfficer",
                NormalizedName = "CREDITOFFICER"
            }
        );

        // Assign roles to users
        modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
            new IdentityUserRole<Guid>
            {
                UserId = Guid.Parse("157e730f-df20-4c5d-be84-22dd80c46efe"),
                RoleId = Guid.Parse("157e730f-df20-4c5d-be84-22dd80c46ef1") // Customer role
            },
            new IdentityUserRole<Guid>
            {
                UserId = Guid.Parse("157e730f-df20-4c5d-be84-22dd80c46ef4"),
                RoleId = Guid.Parse("157e730f-df20-4c5d-be84-22dd80c46ef2") // CreditOfficer role
            }
        );
    }
} 