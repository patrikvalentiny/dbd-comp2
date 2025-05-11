using listings_service.Models;
using ListingsService.Repositories;
using MongoDB.Bson;
using listings_service.Infrastructure.Services;

namespace listings_service.Services
{
    public class ListingService(
        IListingRepository listingRepository,
        MinioStorageRepository storageRepository,
        RedisListingsRepository cacheService,
        ILogger<ListingService> logger)
    {
        private readonly IListingRepository _listingRepository = listingRepository;
        private readonly MinioStorageRepository _storageRepository = storageRepository;
        private readonly RedisListingsRepository _cacheService = cacheService;
        private readonly ILogger<ListingService> _logger = logger;

        // Cache key prefixes
        private const string ALL_LISTINGS_KEY = "listings:all";
        private const string LISTING_KEY_PREFIX = "listing:";
        private const string SELLER_LISTINGS_PREFIX = "listings:seller:";
        private const string SEARCH_RESULTS_PREFIX = "listings:search:";

        public async Task<IEnumerable<Listing>> GetAllListingsAsync()
        {
            // Try to get listings from cache first
            var cachedListings = await _cacheService.GetAsync<IEnumerable<Listing>>(ALL_LISTINGS_KEY);
            if (cachedListings != null)
            {
                _logger.LogInformation("Returned all listings from cache");
                return cachedListings;
            }

            // If not in cache, get from repository
            var listings = await _listingRepository.GetAllAsync();
            
            // Cache the results
            await _cacheService.SetAsync(ALL_LISTINGS_KEY, listings);
            _logger.LogInformation("Cached all listings");
            
            return listings;
        }

        public async Task<Listing?> GetListingByIdAsync(string id)
        {
            // Try to get listing from cache first
            var cacheKey = $"{LISTING_KEY_PREFIX}{id}";
            var cachedListing = await _cacheService.GetAsync<Listing>(cacheKey);
            if (cachedListing != null)
            {
                _logger.LogInformation("Returned listing {ListingId} from cache", id);
                return cachedListing;
            }

            // If not in cache, get from repository
            var listing = await _listingRepository.GetByIdAsync(id);
            
            // Cache the result if found
            if (listing != null)
            {
                await _cacheService.SetAsync(cacheKey, listing);
                _logger.LogInformation("Cached listing {ListingId}", id);
            }
            
            return listing;
        }

        public async Task<IEnumerable<Listing>> GetListingsBySellerIdAsync(string sellerId)
        {
            // Try to get seller listings from cache first
            var cacheKey = $"{SELLER_LISTINGS_PREFIX}{sellerId}";
            var cachedListings = await _cacheService.GetAsync<IEnumerable<Listing>>(cacheKey);
            if (cachedListings != null)
            {
                _logger.LogInformation("Returned seller {SellerId} listings from cache", sellerId);
                return cachedListings;
            }

            // If not in cache, get from repository
            var listings = await _listingRepository.GetBySellerIdAsync(sellerId);
            
            // Cache the results
            await _cacheService.SetAsync(cacheKey, listings);
            _logger.LogInformation("Cached seller {SellerId} listings", sellerId);
            
            return listings;
        }

        public async Task<IEnumerable<Listing>> SearchListingsAsync(string query, string category, string condition, decimal? minPrice, decimal? maxPrice)
        {
            // Create cache key based on search parameters
            var cacheKey = $"{SEARCH_RESULTS_PREFIX}{query}:{category}:{condition}:{minPrice}:{maxPrice}";
            
            // Try to get search results from cache first
            var cachedResults = await _cacheService.GetAsync<IEnumerable<Listing>>(cacheKey);
            if (cachedResults != null)
            {
                _logger.LogInformation("Returned search results from cache for query: {Query}", query);
                return cachedResults;
            }

            // If not in cache, get from repository
            var listings = await _listingRepository.SearchAsync(query, category, condition, minPrice, maxPrice);
            
            // Cache the results with a shorter expiry (15 minutes for search results)
            await _cacheService.SetAsync(cacheKey, listings, TimeSpan.FromMinutes(15));
            _logger.LogInformation("Cached search results for query: {Query}", query);
            
            return listings;
        }

        public async Task<Listing> CreateListingAsync(Listing listing)
        {
            listing.Id = ObjectId.GenerateNewId().ToString();
            
            // Save to repository
            var createdListing = await _listingRepository.CreateAsync(listing);
            
            // Cache the new listing
            var cacheKey = $"{LISTING_KEY_PREFIX}{createdListing.Id}";
            await _cacheService.SetAsync(cacheKey, createdListing);
            
            // Invalidate related caches
            await _cacheService.RemoveAsync(ALL_LISTINGS_KEY);
            if (!string.IsNullOrEmpty(listing.SellerId))
            {
                await _cacheService.RemoveAsync($"{SELLER_LISTINGS_PREFIX}{listing.SellerId}");
            }
            
            return createdListing;
        }

        public async Task<bool> UpdateListingAsync(string id, Listing listing)
        {
            var existingListing = await _listingRepository.GetByIdAsync(id);
            if (existingListing == null)
                return false;

            listing.Id = id;
            listing.CreatedAt = existingListing.CreatedAt;
            
            // Update in repository
            var success = await _listingRepository.UpdateAsync(id, listing);
            
            if (success)
            {
                // Update in cache
                var cacheKey = $"{LISTING_KEY_PREFIX}{id}";
                await _cacheService.SetAsync(cacheKey, listing);
                
                // Invalidate related caches
                await _cacheService.RemoveAsync(ALL_LISTINGS_KEY);
                if (!string.IsNullOrEmpty(listing.SellerId))
                {
                    await _cacheService.RemoveAsync($"{SELLER_LISTINGS_PREFIX}{listing.SellerId}");
                }
                
                // Invalidate search results cache (they might contain this listing)
                await _cacheService.RemoveByPrefixAsync(SEARCH_RESULTS_PREFIX);
            }
            
            return success;
        }

        public async Task<bool> DeleteListingAsync(string id)
        {
            // Delete from repository (or mark as deleted)
            var success = await _listingRepository.DeleteAsync(id);
            
            if (success)
            {
                // Remove from cache
                await _cacheService.RemoveAsync($"{LISTING_KEY_PREFIX}{id}");
                
                // Invalidate related caches
                await _cacheService.RemoveAsync(ALL_LISTINGS_KEY);
                
                // Get the listing to invalidate seller's cache
                var listing = await _listingRepository.GetByIdAsync(id);
                if (listing != null && !string.IsNullOrEmpty(listing.SellerId))
                {
                    await _cacheService.RemoveAsync($"{SELLER_LISTINGS_PREFIX}{listing.SellerId}");
                }
                
                // Invalidate search results cache
                await _cacheService.RemoveByPrefixAsync(SEARCH_RESULTS_PREFIX);
            }
            
            return success;
        }

        public async Task<bool> UpdateListingStatusAsync(string id, string status)
        {
            // Update status in repository
            var success = await _listingRepository.UpdateStatusAsync(id, status);
            
            if (success)
            {
                // Get the updated listing to update cache
                var listing = await _listingRepository.GetByIdAsync(id);
                if (listing != null)
                {
                    // Update in cache
                    var cacheKey = $"{LISTING_KEY_PREFIX}{id}";
                    await _cacheService.SetAsync(cacheKey, listing);
                    
                    // Invalidate related caches
                    await _cacheService.RemoveAsync(ALL_LISTINGS_KEY);
                    if (!string.IsNullOrEmpty(listing.SellerId))
                    {
                        await _cacheService.RemoveAsync($"{SELLER_LISTINGS_PREFIX}{listing.SellerId}");
                    }
                    
                    // Invalidate search results cache
                    await _cacheService.RemoveByPrefixAsync(SEARCH_RESULTS_PREFIX);
                }
            }
            
            return success;
        }

        public async Task<List<UploadUrlResponse>> GenerateImageUploadUrlsAsync(string listingId, List<string> fileNames)
        {
            // This method doesn't need caching as it's generating new URLs each time
            if (string.IsNullOrEmpty(listingId))
            {
                listingId = ObjectId.GenerateNewId().ToString();
            }

            var responses = new List<UploadUrlResponse>();
            foreach (var fileName in fileNames)
            {
                var response = await _storageRepository.GeneratePresignedUrlForUpload(listingId, fileName);
                var parts = response.Split(["\", ObjectName = \"", "\", ContentType = \""], StringSplitOptions.None);
                var url = parts[0].Replace("{PresignedUrl = \"", "");
                var objectName = parts[1];
                var contentType = parts[2].Replace("\"}", "");
                
                responses.Add(new UploadUrlResponse
                {
                    FileName = fileName,
                    PresignedUrl = url,
                    ObjectName = objectName,
                    ContentType = contentType
                });
            }
            return responses;
        }
    }

    public class UploadUrlResponse
    {
        public string FileName { get; set; }
        public string PresignedUrl { get; set; }
        public string ObjectName { get; set; }
        public string ContentType { get; set; }
    }
}
