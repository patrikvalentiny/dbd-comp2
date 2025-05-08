using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewsService.Domain.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", TypeName = "uuid")]
        public Guid Id { get; set; }
        
        [Required]
        [Column("order_id", TypeName = "uuid")]
        public Guid OrderId { get; set; }
        
        [Required]
        [Column("reviewer_id", TypeName = "uuid")]
        public Guid ReviewerId { get; set; }
        
        [Required]
        [Column("target_id", TypeName = "uuid")]
        public Guid TargetId { get; set; }
        
        [Required]
        [Column("target_type")]
        public string TargetType { get; set; } = string.Empty; // seller, buyer, item
        
        [Required]
        [Range(1, 5)]
        [Column("rating")]
        public int Rating { get; set; }
        
        [Column("comment")]
        public string Comment { get; set; } = string.Empty;
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        [Column("helpful")]
        public int Helpful { get; set; } = 0;
    }
}
