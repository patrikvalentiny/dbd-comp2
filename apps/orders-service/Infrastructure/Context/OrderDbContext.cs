using Microsoft.EntityFrameworkCore;
using OrdersService.Domain.Models;

namespace OrdersService.Infrastructure.Data
{
    public class OrderDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<ShippingInfo> ShippingInfos { get; set; }
        
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.ShippingInfo)
                    .WithOne(s => s.Order)
                    .HasForeignKey<ShippingInfo>(s => s.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            modelBuilder.Entity<ShippingInfo>(entity =>
            {
                entity.ToTable("shipping_infos");
                entity.HasKey(e => e.Id);
            });
        }
    }
}
