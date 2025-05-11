using Microsoft.EntityFrameworkCore;
using ReviewsService.Infrastructure.Data;
using ReviewsService.Infrastructure.Repositories;
using ReviewsService.Services;
using ReviewsService.CQRS.Commands;
using ReviewsService.CQRS.Queries;
using Scalar.AspNetCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ReviewDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")!));

// Register repositories and transaction manager
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<ITransactionManager, TransactionManager>();

// Register CQRS handlers
// Commands
builder.Services.AddScoped<ICreateReviewCommand, CreateReviewCommand>();
builder.Services.AddScoped<IUpdateReviewCommand, UpdateReviewCommand>();
builder.Services.AddScoped<IDeleteReviewCommand, DeleteReviewCommand>();
builder.Services.AddScoped<IMarkReviewAsHelpfulCommand, MarkReviewAsHelpfulCommand>();

// Queries
builder.Services.AddScoped<IGetAllReviewsQuery, GetAllReviewsQuery>();
builder.Services.AddScoped<IGetReviewByIdQuery, GetReviewByIdQuery>();
builder.Services.AddScoped<IGetReviewsByReviewerIdQuery, GetReviewsByReviewerIdQuery>();
builder.Services.AddScoped<IGetReviewsByTargetQuery, GetReviewsByTargetQuery>();
builder.Services.AddScoped<IGetAverageRatingQuery, GetAverageRatingQuery>();

// Register services
builder.Services.AddScoped<IReviewService, ReviewService>();


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
    var dbContext = services.GetRequiredService<ReviewDbContext>();
    dbContext.Database.EnsureCreated();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
