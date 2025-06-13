using CreditManager.Domain.Entities.Credit;
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
    }
}