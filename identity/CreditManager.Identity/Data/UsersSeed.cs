using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CreditManager.Identity.Data;

public static class UsersSeed
{
    public static void SeedUsers(this ModelBuilder modelBuilder)
    {
        var passwordHasher = new PasswordHasher<CreditManagerUser>();

        var mSchwarzmuller = new CreditManagerUser
        {
            Id = Guid.Parse("157e730f-df20-4c5d-be84-22dd80c46efe"),
            UserName = "m.schwarzmuller",
            FirstName = "მაქსიმილიან",
            LastName = "შვარცმიულერი",
            NormalizedUserName = "M.SCHWARZMULLER",
            Email = "m.schwarzmuller@example.com",
            NormalizedEmail = "M.SCHWARZMULLER@EXAMPLE.COM",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };
        mSchwarzmuller.PasswordHash = passwordHasher.HashPassword(mSchwarzmuller, "Customer123!");

        var nChapsas = new CreditManagerUser
        {
            Id = Guid.Parse("157e730f-df20-4c5d-be84-22dd80c46ef4"),
            UserName = "n.chapsas",
            FirstName = "ნიკ",
            LastName = "ჩაპსასი",
            NormalizedUserName = "N.CHAPSAS",
            Email = "n.chapsas@example.com",
            NormalizedEmail = "N.CHAPSAS@EXAMPLE.COM",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };
        nChapsas.PasswordHash = passwordHasher.HashPassword(nChapsas, "Officer123!");

        modelBuilder.Entity<CreditManagerUser>().HasData(mSchwarzmuller, nChapsas);
    }
}