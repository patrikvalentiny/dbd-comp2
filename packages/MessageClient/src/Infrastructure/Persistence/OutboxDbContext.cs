using MessageClient.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace MessageClient.Infrastructure.Persistence;

public class OutboxDbContext : DbContext
{
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    
    public OutboxDbContext(DbContextOptions<OutboxDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OutboxMessage>().HasKey(m => m.Id);
        base.OnModelCreating(modelBuilder);
    }
}