using MessageClient.Interfaces;
using Microsoft.Extensions.Hosting;

namespace MessageClient.Implementation;

public class MessageBackgroundService(MessageHandlerRegistry messageHandlerRegistry, IMessagingClient messagingClient) : BackgroundService, IMessageBackgroundService
{

    public void StartListening()
    {
        Console.WriteLine("Starting to listen to messages.");
        messageHandlerRegistry.RegisterAllHandlers(
            messagingClient
        );
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        StartListening();
        return Task.Delay(Timeout.Infinite, stoppingToken);
    }
}