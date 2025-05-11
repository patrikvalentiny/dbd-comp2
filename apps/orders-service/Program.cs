using Microsoft.EntityFrameworkCore;
using OrdersService.Infrastructure.Data;
using OrdersService.Infrastructure.Repositories;
using OrdersService.Services;
using OrdersService.CQRS.Commands;
using OrdersService.CQRS.Queries;
using OrdersService.MessageHandlers;
using Scalar.AspNetCore;
using MessageClient.Extensions;
using MessageClient.Configuration;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")!));

// Register repositories and transaction manager
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ITransactionManager, TransactionManager>();

// Register MessageClient with RabbitMQ and handlers
builder.Services.AddMessageHandler(
    handlerOptions => {
        // Configure message handler options if needed
    },
    clientOptions => {
        clientOptions.ConnectionString = builder.Configuration.GetConnectionString("RabbitMQ") ?? "host=rabbitmq";
        clientOptions.MessagingProvider = MessagingProvider.RabbitMQ;
    }
);

// Register message handlers
builder.Services.AddScoped<UserDeletedHandler>();

// Register CQRS handlers
// Commands
builder.Services.AddScoped<ICreateOrderCommand, CreateOrderCommand>();
builder.Services.AddScoped<IUpdateOrderStatusCommand, UpdateOrderStatusCommand>();
builder.Services.AddScoped<IUpdateShippingInfoCommand, UpdateShippingInfoCommand>();
builder.Services.AddScoped<IDeleteOrderCommand, DeleteOrderCommand>();

// Queries
builder.Services.AddScoped<IGetAllOrdersQuery, GetAllOrdersQuery>();
builder.Services.AddScoped<IGetOrderByIdQuery, GetOrderByIdQuery>();
builder.Services.AddScoped<IGetOrdersByBuyerIdQuery, GetOrdersByBuyerIdQuery>();
builder.Services.AddScoped<IGetOrdersBySellerIdQuery, GetOrdersBySellerIdQuery>();

// Register services
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    // Create database if it doesn't exist
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<OrderDbContext>();
    dbContext.Database.EnsureCreated();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
