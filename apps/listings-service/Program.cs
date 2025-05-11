using Scalar.AspNetCore;
using listings_service.Infrastructure.Contexts;
using listings_service.Repositories;
using listings_service.Services;
using ListingsService.Repositories;
using listings_service.Infrastructure.Services;
using Microsoft.AspNetCore.Http.Features;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure MongoDB with MongoContext
builder.Services.AddSingleton<MongoContext>();
builder.Services.AddScoped<IListingRepository, MongoListingRepository>();

// Configure Redis Connection
var redisConnectionString = builder.Configuration["RedisSettings:ConnectionString"] ?? "localhost:6379";
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));
builder.Services.AddSingleton<RedisListingsRepository>();

// Register MinIO repository
builder.Services.AddSingleton<MinioStorageRepository>();

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
