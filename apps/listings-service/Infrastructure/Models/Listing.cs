using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace listings_service.Infrastructure.Models
{
    public class Listing
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        public string SellerId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string? Subcategory { get; set; }
        public string Condition { get; set; } = null!;
        public decimal Price { get; set; }
        public string Currency { get; set; } = "USD";
        
        public Location? Location { get; set; }
        
        // Store image URLs from MinIO
        public List<string> Images { get; set; } = new();
        
        public string Status { get; set; } = "active";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public ListingMetadata Metadata { get; set; } = new();
    }

    public class Location
    {
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public double[]? Coordinates { get; set; }
    }

    public class ListingMetadata
    {
        public int Views { get; set; } = 0;
        public int Favorites { get; set; } = 0;
        public Dictionary<string, object>? CustomAttributes { get; set; }
    }
}
