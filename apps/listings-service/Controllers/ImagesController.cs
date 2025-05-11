using listings_service.Infrastructure.Services;
using listings_service.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace listings_service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly MinioStorageRepository _storageService;
        private readonly ILogger<ImagesController> _logger;

        public ImagesController(
            MinioStorageRepository storageService,
            ILogger<ImagesController> logger)
        {
            _storageService = storageService;
            _logger = logger;
        }

        [HttpPost("presigned-url")]
        public async Task<IActionResult> GetPresignedUrl([FromBody] PresignedUrlRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ListingId) || string.IsNullOrWhiteSpace(request.FileName))
            {
                return BadRequest("ListingId and FileName are required");
            }

            try
            {
                // Generate a unique file key
                string fileKey = $"{request.ListingId}/{Guid.NewGuid()}-{request.FileName}";
                
                // Generate presigned URL for upload
                string presignedUrl = await _storageService.GeneratePresignedUrlForUpload(
                    request.ListingId, 
                    request.FileName);

                // Return the URL and metadata
                var response = new PresignedUrlResponse
                {
                    UploadUrl = presignedUrl,
                    FileKey = fileKey,
                    Expiration = DateTime.UtcNow.AddMinutes(15) // Match your MinIO setting
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating presigned URL");
                return StatusCode(500, "Failed to generate upload URL");
            }
        }
    }
}
