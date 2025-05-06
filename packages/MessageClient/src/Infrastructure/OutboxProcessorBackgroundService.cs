using MessageClient.Interfaces;
using Microsoft.Extensions.Hosting;

namespace MessageClient.Infrastructure;

public class OutboxProcessorBackgroundService(IMessageProcessor messageProcessor) : BackgroundService
{
    private readonly IMessageProcessor messageProcessor = messageProcessor;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Outbox processor is running.");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await messageProcessor.ProcessOutboxMessagesAsync(stoppingToken);
            } 
            catch (Exception ex)
            {
                await Console.Error.WriteLineAsync($"An error occurred while processing outbox messages: {ex.Message}");
            }
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
        Console.WriteLine("Outbox processor is stopping.");
    }
}