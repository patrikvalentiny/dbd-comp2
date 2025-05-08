using Microsoft.EntityFrameworkCore;
using ReviewsService.Domain.Models;

namespace ReviewsService.Infrastructure.Data
{
    public class ReviewDbContext(DbContextOptions<ReviewDbContext> options) : DbContext(options)
    {
        public DbSet<Review> Reviews { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("reviews");
                entity.HasKey(e => e.Id);
                
                // Create indexes for frequent queries
                entity.HasIndex(e => e.TargetId);
                entity.HasIndex(e => e.ReviewerId);
                entity.HasIndex(e => new { e.TargetId, e.TargetType });
            });
        }
    }
}
