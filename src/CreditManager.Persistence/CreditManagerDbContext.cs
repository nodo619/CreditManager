using CreditManager.Domain.Entities;
using CreditManager.Domain.Entities.Audit;
using CreditManager.Domain.Entities.Credit;
using Microsoft.EntityFrameworkCore;

namespace CreditManager.Persistence;

public class CreditManagerDbContext : DbContext
{
    public CreditManagerDbContext(
        DbContextOptions<CreditManagerDbContext> options
    ) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("creditmanager");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CreditManagerDbContext).Assembly);
        
        //seeding
        
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity<Guid>>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.DateCreated = DateTime.Now;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedDate = DateTime.Now;
                    break;
            }
        }

        await LogChangesAsync(cancellationToken);
        
        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task LogChangesAsync(CancellationToken cancellationToken)
    {
        var transAuditHs = new List<TransAuditH>();
        var transAuditEs = new List<TransAuditE>();

        foreach (var entry in ChangeTracker.Entries<AuditableEntity<Guid>>())
        {
            if (entry.State == EntityState.Added)
            {
                //TODO
                // need to set correct user id
                entry.Entity.CreatedById = Guid.Empty;
            }

            if (entry.State is EntityState.Detached or EntityState.Unchanged or EntityState.Added)
            {
                continue;
            }
            
            var entityType = entry.Entity.GetType();
            
            var tableName = base.Model.FindEntityType(entityType)!.GetTableName();

            var transAuditH = new TransAuditH
            {
                Id = Guid.NewGuid(),
                Machine = "",
                //TODO correct username
                Username = "",
                OsUser = "",
                PrimKey = entry.Entity.Id.ToString(),
                TableName = tableName!,
                TransDate = DateTime.Now,
                TranType = (int)entry.State
            };
            transAuditHs.Add(transAuditH);

            foreach (var prop in entry.Properties)
            {
                var originalValue = prop.CurrentValue?.ToString() ?? "null";
                var currentValue = prop.CurrentValue?.ToString() ?? "null";

                if (entry.State == EntityState.Deleted)
                {
                    currentValue = null;
                }
                else if (!prop.IsModified)
                {
                    continue;
                }

                if (originalValue == currentValue)
                {
                    continue;
                }
                
                transAuditEs.Add(
                    new TransAuditE
                    {
                        Id = Guid.NewGuid(),
                        FieldName = prop.Metadata.Name,
                        NewValue = currentValue,
                        OldValue = originalValue,
                        TransAuditHId = transAuditH.Id
                    }
                );
            }
        }
        
        await TransAuditHs.AddRangeAsync(transAuditHs, cancellationToken);
        await TransAuditEs.AddRangeAsync(transAuditEs, cancellationToken);
    }

    private DbSet<TransAuditE> TransAuditEs => null!;

    private DbSet<TransAuditH> TransAuditHs => null!;
    
    public DbSet<CreditRequest> CreditRequests { get; set; }
}