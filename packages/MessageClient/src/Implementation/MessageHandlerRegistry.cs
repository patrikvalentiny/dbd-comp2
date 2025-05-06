using System.Linq.Expressions;
using MessageClient.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MessageClient.Implementation;

public class MessageHandlerRegistry(IServiceProvider serviceProvider)
{

    public void RegisterAllHandlers(IMessagingClient messagingClient, string subscriptionPrefix = "")
    {
        // Get all concrete types in all loaded assemblies
        var allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(t => t.IsClass && !t.IsAbstract).ToList();
        
        foreach (var type in allTypes)
        {
            // Check if it implements IMessageHandler<T>    
            var messageHandlerInterfaces = type.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMessageHandler<>));
            
            foreach(var handlerInterface in messageHandlerInterfaces)
            {
                // handlerInterface is e.g. IMessageHandler<SomeMessageType>
                var messageType = handlerInterface.GetGenericArguments()[0];
                Console.WriteLine($"Found handler {type.Name} for message type {messageType.Name}");

                // Generate a dynamic subscription for the message type
                // Use reflection to call IMessagingClient.SubscribeAsync<T>(...)
                var subscribeAsyncMethods = typeof(IMessagingClient).GetMethods()
                    .Where(m => m.Name == nameof(IMessagingClient.SubscribeAsync)).ToList();
                
                // Pick the correct SubscribeAsync<T> method
                var subscribeAsyncMethod = subscribeAsyncMethods.FirstOrDefault(m =>
                {
                    var parameters = m.GetParameters();
                    return parameters.Length == 3 
                           && parameters[0].ParameterType == typeof(string) 
                           && parameters[1].Name.StartsWith("handler")
                           && parameters[2].ParameterType == typeof(CancellationToken);
                });
                if (subscribeAsyncMethod == null)
                {
                    Console.WriteLine($"Could not find matching SubscribeAsync<T> method for message type {messageType.Name}");
                    continue;
                }
                
                // Make the subscribe method generic for the message type
                var genericSubscribeMethod = subscribeAsyncMethod.MakeGenericMethod(messageType);
                
                // Build the delegate: (T message) => ???.Handler(message, CancellationToken)
                // Use reflection to create an instance of the handler
                var handlerInstance = ActivatorUtilities.CreateInstance(serviceProvider, type);

                if (handlerInstance == null)
                {
                    Console.WriteLine($"Could not create instance of {type.Name}");
                    continue;
                }
                
                // Get the Handler method from the handler interface
                var methodInfo = handlerInterface.GetMethod("Handler");
                if (methodInfo == null)
                {
                    Console.WriteLine($"No Handler method found in {handlerInterface.Name}");
                    continue;
                }

                // Create a typed delegate for the handler method
                Func<object, Task> typedFunc = msg =>
                {
                    return (Task)methodInfo.Invoke(handlerInstance, new object[] { msg, CancellationToken.None });
                };
                
                // SubscribeAsync<T> expects a `Action<T>`
                // Using a reflection "trick" to create a lambda expression
                var param = Expression.Parameter(messageType, "message");
                var expr = Expression.Invoke(Expression.Constant(typedFunc), param);
                var lambdaType = typeof(Action<>).MakeGenericType(messageType);
                var lambda = Expression.Lambda(lambdaType, expr, param).Compile();
                
                // Build a unique subscriptionId
                var subscriptionId = $"{subscriptionPrefix}_{messageType.Name}";
                
                // Finally, call the generic SubscribeAsync<T> method
                genericSubscribeMethod.Invoke(
                    obj: messagingClient, 
                    parameters: new object[] { subscriptionId, lambda, CancellationToken.None });
            }
        }
    }
}