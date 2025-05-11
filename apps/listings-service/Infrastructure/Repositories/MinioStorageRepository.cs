using Minio;
using Minio.DataModel.Args;
using System.Text.RegularExpressions;

namespace listings_service.Infrastructure.Services
{
    public class MinioStorageRepository
    {
        private readonly ILogger<MinioStorageRepository> _logger;
        private readonly IMinioClient _minioClient;
        private readonly string _bucketName;
        private readonly int _presignedUrlExpiryMinutes;

        public MinioStorageRepository(IConfiguration configuration, ILogger<MinioStorageRepository> logger)
        {
            _logger = logger;
            
            var minioSettings = configuration.GetSection("MinioSettings");
            var endpoint = minioSettings["Endpoint"] ?? "localhost:9000";
            var accessKey = minioSettings["AccessKey"] ?? "minioadmin";
            var secretKey = minioSettings["SecretKey"] ?? "minioadmin";
            var useSSL = bool.Parse(minioSettings["UseSSL"] ?? "false");
            _bucketName = minioSettings["BucketName"] ?? "listing-images";
            _presignedUrlExpiryMinutes = int.Parse(minioSettings["PresignedUrlExpiryInMinutes"] ?? "15");

            _minioClient = new MinioClient()
                .WithEndpoint(endpoint)
                .WithCredentials(accessKey, secretKey)
                .WithSSL(useSSL)
                .Build();

        }

        public async Task<string> GeneratePresignedUrlForUpload(string listingId, string fileName)
        {
            try
            {
                // Sanitize filename and create object path
                string sanitizedFileName = SanitizeFileName(fileName);
                string objectName = $"{listingId}/{Guid.NewGuid()}-{sanitizedFileName}";
                string contentType = GetContentTypeByExtension(Path.GetExtension(fileName));

                // Create presigned URL for PUT operation
                var presignedPutObjectArgs = new PresignedPutObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(objectName)
                    .WithExpiry(_presignedUrlExpiryMinutes * 60);

                string presignedUrl = await _minioClient.PresignedPutObjectAsync(presignedPutObjectArgs);
                
                _logger.LogInformation($"Generated presigned upload URL for {objectName}");
                
                // Return both the URL and the object name so the client knows where the file will be stored
                return new { PresignedUrl = presignedUrl, ObjectName = objectName, ContentType = contentType }.ToString()!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating presigned upload URL");
                throw;
            }
        }

        public async Task<string> GeneratePresignedUrlForDownload(string objectName)
        {
            try
            {
                var presignedGetObjectArgs = new PresignedGetObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(objectName)
                    .WithExpiry(_presignedUrlExpiryMinutes * 60);

                string presignedUrl = await _minioClient.PresignedGetObjectAsync(presignedGetObjectArgs);
                
                _logger.LogInformation($"Generated presigned download URL for {objectName}");
                
                return presignedUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating presigned download URL");
                throw;
            }
        }

        public string GetImageUrl(string objectName)
        {
            var config = _minioClient.Config;
            string protocol = config.Secure ? "https" : "http";
            return $"{protocol}://{config.Endpoint}/{_bucketName}/{objectName}";
        }

        private string SanitizeFileName(string fileName)
        {
            // Replace spaces with dashes and remove special characters
            string sanitized = Regex.Replace(fileName, @"[^\w\.-]", "-");
            // Ensure no double dashes
            sanitized = Regex.Replace(sanitized, "-+", "-");
            return sanitized.ToLower();
        }

        private string GetContentTypeByExtension(string extension)
        {
            extension = extension.ToLower();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };
        }

        public async Task EnsureBucketExistsAsync()
        {
            try
            {
                // Check if bucket exists
                var bucketExistsArgs = new BucketExistsArgs().WithBucket(_bucketName);
                bool found = await _minioClient.BucketExistsAsync(bucketExistsArgs);
                if (!found)
                {
                    // Create bucket if it doesn't exist
                    var makeBucketArgs = new MakeBucketArgs().WithBucket(_bucketName);
                    await _minioClient.MakeBucketAsync(makeBucketArgs);
                    _logger.LogInformation($"Created bucket: {_bucketName}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error ensuring bucket {_bucketName} exists");
                throw;
            }
        }
    }
}
