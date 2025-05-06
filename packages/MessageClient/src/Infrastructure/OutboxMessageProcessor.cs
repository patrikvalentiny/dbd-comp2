using System.Text.Json;
using MessageClient.Infrastructure.Persistence;
using MessageClient.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MessageClient.Infrastructure;

public class OutboxMessageProcessor(IServiceScopeFactory serviceScopeFactory, IMessagingClient messagingClient) : IMessageProcessor
{

    public async Task ProcessOutboxMessagesAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OutboxDbContext>();
        var messages = await dbContext.OutboxMessages
            .Where(m => m.ProcessedAtUtc == null)
            .OrderBy(m => m.CreatedAtUtc)
            .Take(20)
            .ToListAsync(cancellationToken);
        
        if(messages.Count == 0)
            return;
        
        Console.WriteLine($"Processing {messages.Count} outbox messages");

        foreach (var message in messages)
        {
            try
            {
                var messageType = Type.GetType(message.Type);
                if(messageType is null)
                    throw new InvalidOperationException($"Unknown message type: {message.Type}");
                var payload = JsonSerializer.Deserialize(message.Payload, messageType);

                var publishMethod =
                    typeof(IMessagingClient).GetMethod(nameof(IMessagingClient.PublishAsync));
                
                if(publishMethod is null)
                    throw new InvalidOperationException("PublishAsync method not found");
                
                var genericPublishMethod = publishMethod.MakeGenericMethod(
                        messageType);
                
                Task publish = (Task)genericPublishMethod.Invoke(messagingClient, new [] {message.Subscription, payload, cancellationToken });
                await publish?.ContinueWith(async task =>
                {
                    Console.WriteLine($"Outbox message {message.Id} processed {task.Status}");
                    if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
                    {
                        message.ProcessedAtUtc = DateTime.UtcNow;
                        await dbContext.SaveChangesAsync(cancellationToken);
                    }

                    if (task.IsFaulted || task.IsCanceled)
                    {
                        Console.Error.WriteLine($"An error occurred while processing outbox message {message.Id}: {task.Exception?.Message}");
                    }
                }, cancellationToken)!;
            } 
            catch (Exception ex)
            {
                await Console.Error.WriteLineAsync($"An error occurred while processing outbox message {message.Id}: {ex.Message}");
            }
        }
    }
}