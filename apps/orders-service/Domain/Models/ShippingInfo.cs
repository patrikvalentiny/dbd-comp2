using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrdersService.Domain.Models
{
    public class ShippingInfo
    {
        [Key]
        [Column("id", TypeName = "uuid")]
        public Guid Id { get; set; }
        
        [Required]
        [Column("order_id", TypeName = "uuid")]
        public Guid OrderId { get; set; }
        
        [Required]
        [Column("address_line1")]
        public string AddressLine1 { get; set; } = string.Empty;
        
        [Column("address_line2")]
        public string? AddressLine2 { get; set; }
        
        [Required]
        [Column("city")]
        public string City { get; set; } = string.Empty;
        
        [Required]
        [Column("state")]
        public string State { get; set; } = string.Empty;
        
        [Required]
        [Column("postal_code")]
        public string PostalCode { get; set; } = string.Empty;
        
        [Required]
        [Column("country")]
        public string Country { get; set; } = string.Empty;
        
        [Column("shipping_method")]
        public string? ShippingMethod { get; set; }
        
        [Column("tracking_number")]
        public string? TrackingNumber { get; set; }
        
        [Column("estimated_delivery")]
        public DateTime? EstimatedDelivery { get; set; }
        
        // Navigation property
        [ForeignKey("OrderId")]
        public Order? Order { get; set; }
    }
}
