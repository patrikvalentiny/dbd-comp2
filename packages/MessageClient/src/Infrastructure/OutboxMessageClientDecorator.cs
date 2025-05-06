using System.Text.Json;
using MessageClient.Infrastructure.Models;
using MessageClient.Infrastructure.Persistence;
using MessageClient.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MessageClient.Infrastructure;

public class OutboxMessageClientDecorator(IServiceScopeFactory scopeFactory, IMessagingClient messagingClient, bool isOutboxEnabled) : IMessagingClient
{

    public async Task PublishAsync<T>(string subscription, T message, CancellationToken cancellationToken = default)
    {
        if (isOutboxEnabled)
        {
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<OutboxDbContext>();
            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Subscription = subscription,
                CreatedAtUtc = DateTime.UtcNow,
                Type = typeof(T).AssemblyQualifiedName!,
                Payload = JsonSerializer.Serialize(message)
            };
            Console.WriteLine($"Publishing message to outbox: {outboxMessage.Id}");
            await dbContext.OutboxMessages.AddAsync(outboxMessage, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        } else
        {
            await messagingClient.PublishAsync(subscription, message, cancellationToken);
        }
    }

    public async Task UnsubscribeAsync(string subscription, CancellationToken cancellationToken = default)
    {
        await messagingClient.UnsubscribeAsync(subscription, cancellationToken);
    }

    public async Task SubscribeAsync<T>(string subscription, Action<T> handler, CancellationToken cancellationToken = default)
    {
        await messagingClient.SubscribeAsync(subscription, handler, cancellationToken);
    }
}