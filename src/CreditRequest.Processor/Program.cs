using CreditManager.Infrastructure.Messaging;
using CreditRequest.Processor;
using MassTransit;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration;

        services.AddMassTransit(x =>
        {
            x.AddConsumers(typeof(CreditRequestMessageConsumer).Assembly);
            
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(config["RabbitMQ:Host"], h =>
                {
                    h.Username(config["RabbitMQ:Username"] ?? string.Empty);
                    h.Password(config["RabbitMQ:Password"] ?? string.Empty);
                });
                
                cfg.ReceiveEndpoint("credit-request-queue", e =>
                {
                    e.ConfigureConsumers(ctx);
                });
            });
        });
        
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();