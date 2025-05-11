using listings_service.Models;
using ListingsService.Repositories;
using MongoDB.Bson;
using listings_service.Infrastructure.Services;

namespace listings_service.Services
{
    public class ListingService(IListingRepository listingRepository, MinioStorageRepository storageService)
    {

        public async Task<IEnumerable<Listing>> GetAllListingsAsync()
        {
            return await listingRepository.GetAllAsync();
        }

        public async Task<Listing?> GetListingByIdAsync(string id)
        {
            var listing = await listingRepository.GetByIdAsync(id);
            return listing;
        }

        public async Task<IEnumerable<Listing>> GetListingsBySellerIdAsync(string sellerId)
        {
            return await listingRepository.GetBySellerIdAsync(sellerId);
        }

        public async Task<IEnumerable<Listing>> SearchListingsAsync(string query, string category, string condition, decimal? minPrice, decimal? maxPrice)
        {
            return await listingRepository.SearchAsync(query, category, condition, minPrice, maxPrice);
        }

        public async Task<Listing> CreateListingAsync(Listing listing, IEnumerable<IFormFile>? imageFiles = null)
        {
            listing.Id = ObjectId.GenerateNewId().ToString();
            
            // Process image files if any
            if (imageFiles != null && imageFiles.Any())
            {
                listing.Images = [];
                
                foreach (var file in imageFiles)
                {
                    if (file.Length > 0)
                    {
                        // Generate a unique file name based on listing ID and file name
                        var fileName = $"{listing.Id}/{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                        
                        // Upload the file to Minio
                        using var stream = file.OpenReadStream();
                        var contentType = file.ContentType;
                        await storageService.UploadFileAsync(fileName, stream, contentType);
                        
                        // Add the file reference to the listing's images collection
                        listing.Images.Add(fileName);
                    }
                }
            }
            
            return await listingRepository.CreateAsync(listing);
        }

        public async Task<bool> UpdateListingAsync(string id, Listing listing, IEnumerable<IFormFile>? imageFiles = null)
        {
            var existingListing = await listingRepository.GetByIdAsync(id);
            if (existingListing == null)
                return false;

            listing.Id = id;
            listing.CreatedAt = existingListing.CreatedAt;
            
            // Process new image files if any
            if (imageFiles != null && imageFiles.Any())
            {
                // Initialize images collection if it doesn't exist
                listing.Images ??= [];
                
                foreach (var file in imageFiles)
                {
                    if (file.Length > 0)
                    {
                        // Generate a unique file name
                        var fileName = $"{listing.Id}/{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                        
                        // Upload the file to Minio
                        using var stream = file.OpenReadStream();
                        var contentType = file.ContentType;
                        await storageService.UploadFileAsync(fileName, stream, contentType);
                        
                        // Add the file reference to the listing's images collection
                        listing.Images.Add(fileName);
                    }
                }
            }
            
            return await listingRepository.UpdateAsync(id, listing);
        }

        public async Task<bool> DeleteListingAsync(string id)
        {
            return await listingRepository.DeleteAsync(id);
        }

        public async Task<bool> UpdateListingStatusAsync(string id, string status)
        {
            return await listingRepository.UpdateStatusAsync(id, status);
        }
    }
}
