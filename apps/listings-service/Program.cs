using Scalar.AspNetCore;
using listings_service.Infrastructure.Contexts;
using listings_service.Repositories;
using listings_service.Services;
using ListingsService.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure MongoDB with MongoContext
builder.Services.AddSingleton<MongoContext>();
builder.Services.AddScoped<IListingRepository, MongoListingRepository>();
builder.Services.AddScoped<ListingService>();

var app = builder.Build();

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
