using Microsoft.Extensions.DependencyInjection;
using MessageClient.Configuration;
using MessageClient.Factories;
using MessageClient.Interfaces;
using MessageClient.Implementation;
using MessageClient.Infrastructure;
using MessageClient.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MessageClient.Extensions;

public static class ServiceCollectionExtension {
    
    public static IServiceCollection AddMessageClient(
        this IServiceCollection services,
        Action<MessageClientOptions> configureOptions
    ){
        MessageClientOptions options = new MessageClientOptions();
        configureOptions(options);

        IMessagingClient? messagingClient;
        switch(options.MessagingProvider){
            case MessagingProvider.RabbitMQ:
                messagingClient = RabbitMQFactory.Create(options);
                services.AddSingleton<IMessagingClient>(
                    sp => messagingClient
                );
                break;
            default:
                throw new ArgumentException("");
        }

        if (options.UseOutbox)
        {
            services.AddPersistence();
            services.AddOutboxProcessor(messagingClient);
        }
        
        return services;
    }
    
    public static IServiceCollection AddMessageHandler(
        this IServiceCollection services,
        Action<MessageHandlerOptions> configureOptions,
        Action<MessageClientOptions> configureClientOptions
    ){
        MessageHandlerOptions options = new MessageHandlerOptions();
        configureOptions(options);
        
        services.AddMessageClient(configureClientOptions);
        services.AddSingleton<MessageHandlerRegistry>();
        services.AddHostedService<MessageBackgroundService>();
        
        return services;
    }

    public static IServiceCollection AddOutboxProcessor(this IServiceCollection services,
        IMessagingClient innerMessagingClient)
    {
        services.AddSingleton<IMessagingClient, OutboxMessageClientDecorator>(sp =>
        {
            var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
            return new OutboxMessageClientDecorator(scopeFactory, innerMessagingClient, true);
        });
        services.AddSingleton<IMessageProcessor, OutboxMessageProcessor>(
            sp => new OutboxMessageProcessor(sp.GetRequiredService<IServiceScopeFactory>(), innerMessagingClient));
        services.AddHostedService<OutboxProcessorBackgroundService>();
        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddDbContext<OutboxDbContext>(options => options.UseSqlite("Data Source=outbox.db"));
        return services;
    }
}