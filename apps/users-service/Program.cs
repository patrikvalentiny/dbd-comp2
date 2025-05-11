using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using UsersService.Infrastructure.Data;
using UsersService.Infrastructure.Repositories;
using UsersService.Services;
using UsersService.CQRS.Commands;
using UsersService.CQRS.Queries;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")!));

// Register repositories and transaction manager
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITransactionManager, TransactionManager>();

// Register CQRS handlers
// Commands
builder.Services.AddScoped<ICreateUserCommand, CreateUserCommand>();
builder.Services.AddScoped<IUpdateUserCommand, UpdateUserCommand>();
builder.Services.AddScoped<IDeleteUserCommand, DeleteUserCommand>();

// Queries
builder.Services.AddScoped<IGetAllUsersQuery, GetAllUsersQuery>();
builder.Services.AddScoped<IGetUserByIdQuery, GetUserByIdQuery>();
builder.Services.AddScoped<IGetUserByUsernameQuery, GetUserByUsernameQuery>();
builder.Services.AddScoped<IGetUserByEmailQuery, GetUserByEmailQuery>();

// Register services
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<UserDbContext>();
    dbContext.Database.EnsureCreated();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
