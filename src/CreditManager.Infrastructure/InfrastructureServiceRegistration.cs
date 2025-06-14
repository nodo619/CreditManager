using CreditManager.Infrastructure.Messaging;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CreditManager.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
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
}