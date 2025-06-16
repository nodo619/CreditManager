using CreditManager.Domain.Entities.Credit;
using CreditManager.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreditManager.Persistence.Configurations;

public class CreditRequestConfiguration : IEntityTypeConfiguration<CreditRequest>
{
    public void Configure(EntityTypeBuilder<CreditRequest> builder)
    {
        builder.ToTable("CreditRequests");
        
        builder.HasKey(x => x.Id);

        builder.Property(c => c.Comments)
            .HasMaxLength(2000);

        builder.Property(c => c.CurrencyCode)
            .HasMaxLength(3);

        builder.Property(c => c.Amount)
            .HasPrecision(18, 2);

        builder
            .HasOne<SentCreditRequest>()
            .WithOne(s => s.CreditRequest)
            .HasForeignKey<SentCreditRequest>(s => s.CreditRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(c => c.Customer)
            .WithMany()
            .HasForeignKey(c => c.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}