using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace listings_service.Models;
    public class Listing
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string SellerId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Status { get; set; } = "active"; // active, sold, inactive
        public List<string> Images { get; set; } = [];
        
        [BsonDateTimeOptions]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [BsonDateTimeOptions]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
    
