using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrdersService.Domain.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", TypeName = "uuid")]
        public Guid Id { get; set; }
        
        [Required]
        [Column("buyer_id", TypeName = "uuid")]
        public Guid BuyerId { get; set; }
        
        [Required]
        [Column("seller_id", TypeName = "uuid")]
        public Guid SellerId { get; set; }
        
        [Required]
        [Column("listing_id")]
        public string ListingId { get; set; } = string.Empty;
        
        [Required]
        [Column("status")]
        public string Status { get; set; } = "pending";
        
        [Required]
        [Column("amount", TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        
        [Required]
        [Column("currency")]
        public string Currency { get; set; } = "USD";
        
        [Column("payment_id")]
        public string? PaymentId { get; set; }
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public ShippingInfo? ShippingInfo { get; set; }
    }
}
