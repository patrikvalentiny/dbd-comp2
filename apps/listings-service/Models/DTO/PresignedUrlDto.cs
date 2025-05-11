namespace listings_service.Models.DTO
{
    public class PresignedUrlRequest
    {
        public string ListingId { get; set; } = "";
        public string FileName { get; set; } = "";
    }

    public class PresignedUrlResponse
    {
        public string UploadUrl { get; set; } = "";
        public string FileKey { get; set; } = "";
        public DateTime Expiration { get; set; }
    }

    public class UploadCompletedRequest
    {
        public string ListingId { get; set; } = "";
        public string FileKey { get; set; } = "";
    }
}
