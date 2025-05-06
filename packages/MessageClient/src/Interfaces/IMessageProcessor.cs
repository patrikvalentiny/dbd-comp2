namespace MessageClient.Interfaces;

public interface IMessageProcessor
{
    Task ProcessOutboxMessagesAsync(CancellationToken cancellationToken);
}