using EasyNetQ;
using MessageClient.Interfaces;

namespace MessageClient.Implementation;

public class RabbitMQDriver(IBus bus) : IMessagingClient {
    private readonly Dictionary<string, IDisposable> _subscriptions = [];

    public async Task SubscribeAsync<T>(string subscription, Action<T> handler, CancellationToken cancellationToken = default){
        if(_subscriptions.Keys.Contains(subscription)){
            throw new ArgumentException("");
        }
        var subscriptionHandle = await bus.PubSub.SubscribeAsync<T>(
            subscription,
            handler,
            cancellationToken
        );
        _subscriptions.Add(subscription, subscriptionHandle);
    }

    public async Task PublishAsync<T>(string subscription, T message, CancellationToken cancellationToken = default){
        Console.WriteLine($"Publishing message to {subscription}");
        await bus.PubSub.PublishAsync<T>(message, cancellationToken);
    }

    public Task UnsubscribeAsync(string subscription, CancellationToken cancellationToken = default){
        if(_subscriptions.TryGetValue(subscription, out var subscriptionHandle)){
            subscriptionHandle.Dispose();
            _subscriptions.Remove(subscription);
        }
        return Task.CompletedTask;
    }
}