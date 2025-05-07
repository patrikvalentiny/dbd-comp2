using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace listings_service.Models
{
    public class Listing
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string SellerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public string Condition { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public Location Location { get; set; }
        public List<string> Images { get; set; } = [];
        public string Status { get; set; } = "active"; // active, sold, reserved, deleted
        
        [BsonDateTimeOptions]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [BsonDateTimeOptions]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public ListingMetadata Metadata { get; set; } = new ListingMetadata();
    }

    public class Location
    {
        public string City { get; set; }
        public string Country { get; set; }
        
        [BsonElement("coordinates")]
        public double[] Coordinates { get; set; } = new double[2];
    }

    public class ListingMetadata
    {
        public int Views { get; set; } = 0;
        public int Favorites { get; set; } = 0;
        
        [BsonElement("custom_attributes")]
        public Dictionary<string, object> CustomAttributes { get; set; } = [];
    }
}
