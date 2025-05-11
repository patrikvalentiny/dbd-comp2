using Scalar.AspNetCore;
using listings_service.Infrastructure.Contexts;
using listings_service.Repositories;
using listings_service.Services;
using ListingsService.Repositories;
using listings_service.Infrastructure.Services;
using Microsoft.AspNetCore.Http.Features;
using StackExchange.Redis;
using listings_service.CQRS.Commands;
using listings_service.CQRS.Queries;
using listings_service.MessageHandlers;
using MessageClient.Extensions;
using MessageClient.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure MongoDB with MongoContext
builder.Services.AddSingleton<MongoContext>();
builder.Services.AddScoped<IListingRepository, MongoListingRepository>();
builder.Services.AddScoped<IMongoTransactionManager, MongoTransactionManager>();

// Configure Redis Connection
var redisConnectionString = builder.Configuration["RedisSettings:ConnectionString"] ?? "localhost:6379";
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));
builder.Services.AddSingleton<RedisListingsRepository>();

// Register MinIO repository
builder.Services.AddSingleton<MinioStorageRepository>();

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

// Register CQRS components
// Commands
builder.Services.AddScoped<ICreateListingCommand, CreateListingCommand>();
builder.Services.AddScoped<IUpdateListingCommand, UpdateListingCommand>();
builder.Services.AddScoped<IDeleteListingCommand, DeleteListingCommand>();
builder.Services.AddScoped<IUpdateListingStatusCommand, UpdateListingStatusCommand>();
builder.Services.AddScoped<IGenerateImageUploadUrlsCommand, GenerateImageUploadUrlsCommand>();

// Queries
builder.Services.AddScoped<IGetAllListingsQuery, GetAllListingsQuery>();
builder.Services.AddScoped<IGetListingByIdQuery, GetListingByIdQuery>();
builder.Services.AddScoped<IGetListingsBySellerIdQuery, GetListingsBySellerIdQuery>();
builder.Services.AddScoped<ISearchListingsQuery, SearchListingsQuery>();

// Register services
builder.Services.AddScoped<ListingService>();

// Configure form options for file uploads
builder.Services.Configure<FormOptions>(options => 
{
    options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10 MB
});

var app = builder.Build();

// Ensure MinIO bucket exists
using (var scope = app.Services.CreateScope())
{
    var minioRepository = scope.ServiceProvider.GetRequiredService<MinioStorageRepository>();
    await minioRepository.EnsureBucketExistsAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
