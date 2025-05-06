namespace MessageClient.Interfaces;

public interface IMessageHandler<in T>
{
    public Task Handler(T message, CancellationToken cancellationToken);
}