namespace MessageClient.Interfaces;

public interface IMessagingClient {
    Task SubscribeAsync<T>(string subscription, Action<T> handler, CancellationToken cancellationToken = default);
    Task PublishAsync<T>(string subscription, T message, CancellationToken cancellationToken = default);
    Task UnsubscribeAsync(string subscription, CancellationToken cancellationToken = default);
}