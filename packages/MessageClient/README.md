# Message Client

A message client for sending and receiving messages using message brokers like RabbitMQ.

## Getting Started

### Prerequisites

- .NET 7.0
- EasyNetQ
- Newtonsoft.JSON
- Microsoft.Extensions.DependencyInjection.Abstractions

### Installation

Add the package to your solution.

```bash
dotnet sln add ../../packages/MessageClient/src/MessageClient.csproj
```

And add the package to your project.

```bash
dotnet add reference ../../../packages/MessageClient/src/MessageClient.csproj
```

## Usage

After adding the package to your solution, register the messaging client in your `Program.cs` file.

```csharp
// ...
using MessageClient.Extensions;
using MessageClient.Configuration;
// ...
var builder = WebApplication.CreateBuilder(args);

// ...

builder.Services.AddMessageClient(options => {
    options.ConnectionString = "host=localhost";
    options.MessagingProvider = MessagingProvider.RabbitMQ;
});

// ...
```
