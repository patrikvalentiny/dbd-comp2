using EasyNetQ;
using MessageClient.Configuration;
using MessageClient.Implementation;

namespace MessageClient.Factories;

public static class RabbitMQFactory {
    public static RabbitMQDriver Create(MessageClientOptions options) {
        IBus bus = RabbitHutch.CreateBus(options.ConnectionString + ";publisherConfirms=true;timeout=10");
        return new RabbitMQDriver(bus);
    }
}