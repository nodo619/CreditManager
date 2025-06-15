using CreditManager.Infrastructure;
using CreditManager.Persistence;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration;

        services.AddPersistenceServices(config);
        services.AddMassTransitWithConsumers(config);

    })
    .Build();

await host.RunAsync();