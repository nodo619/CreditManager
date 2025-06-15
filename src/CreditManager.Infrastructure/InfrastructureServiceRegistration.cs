using CreditManager.Application.Contracts.Infrastructure;
using CreditManager.Infrastructure.Messaging;
using CreditManager.Infrastructure.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CreditManager.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddCommonInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        
        return services;
    }
    
    public static IServiceCollection AddMassTransitWithConsumers(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumers(typeof(CreditRequestMessageConsumer).Assembly);

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(configuration["RabbitMQ:Host"], h =>
                {
                    h.Username(configuration["RabbitMQ:Username"] ?? string.Empty);
                    h.Password(configuration["RabbitMQ:Password"] ?? string.Empty);
                });

                cfg.ReceiveEndpoint("credit-request-queue", e =>
                {
                    e.ConfigureConsumers(ctx);
                });
            });
        });

        return services;
    }
    
    public static IServiceCollection AddMassTransitBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(configuration["RabbitMQ:Host"], h =>
                {
                    h.Username(configuration["RabbitMQ:Username"] ?? string.Empty);
                    h.Password(configuration["RabbitMQ:Password"] ?? string.Empty);
                });
            });
        });

        return services;
    }
}