using CreditManager.Application.Contracts.Persistence;
using CreditManager.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CreditManager.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<CreditManagerDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("CreditManager"))
        );
        
        services.AddScoped(typeof(IAsyncRepository<,>), typeof(BaseRepository<,>));
        
        return services;
    }
}