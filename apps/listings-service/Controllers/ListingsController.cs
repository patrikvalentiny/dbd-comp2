using listings_service.Models;
using listings_service.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace listings_service.Controllers
{
    /// <summary>
    /// API controller for managing listings in the second-hand e-commerce platform.
    /// Provides endpoints for creating, retrieving, updating, and deleting product listings,
    /// as well as searching and filtering listings by various criteria.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ListingsController : ControllerBase
    {
        private readonly ListingService _listingService;

        public ListingsController(ListingService listingService)
        {
            _listingService = listingService;
        }

        /// <summary>
        /// Retrieves all active listings in the system.
        /// </summary>
        /// <returns>A collection of all listings available on the platform.</returns>
        /// <response code="200">Returns the list of listings</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EndpointName("GetAllListings")]
        [EndpointSummary("Retrieves all listings in the system.")]
        [EndpointDescription("Returns a collection of all product listings with complete details.")]
        public async Task<ActionResult<IEnumerable<Listing>>> GetAll()
        {
            var listings = await _listingService.GetAllListingsAsync();
            return Ok(listings);
        }

        /// <summary>
        /// Retrieves a specific listing by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the listing to retrieve.</param>
        /// <returns>The listing with the specified ID.</returns>
        /// <response code="200">Returns the requested listing</response>
        /// <response code="404">If the listing with the specified ID doesn't exist</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EndpointName("GetListingById")]
        [EndpointSummary("Retrieves a specific listing by ID.")]
        [EndpointDescription("Fetches the complete details of a single listing specified by its unique identifier.")]
        public async Task<ActionResult<Listing>> GetById(string id)
        {
            var listing = await _listingService.GetListingByIdAsync(id);
            if (listing == null)
            {
                return NotFound();
            }
            return Ok(listing);
        }

        /// <summary>
        /// Retrieves all listings posted by a specific seller.
        /// </summary>
        /// <param name="sellerId">The unique identifier of the seller.</param>
        /// <returns>A collection of listings posted by the specified seller.</returns>
        /// <response code="200">Returns the list of listings for the specified seller</response>
        [HttpGet("seller/{sellerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EndpointName("GetListingsBySeller")]
        [EndpointSummary("Retrieves all listings for a specific seller.")]
        [EndpointDescription("Returns a collection of listings that were posted by the seller with the specified ID.")]
        public async Task<ActionResult<IEnumerable<Listing>>> GetByUserId(string sellerId)
        {
            var listings = await _listingService.GetListingsBySellerIdAsync(sellerId);
            return Ok(listings);
        }

        /// <summary>
        /// Searches and filters listings based on query parameters.
        /// </summary>
        /// <param name="query">The search term to match against listing titles and descriptions.</param>
        /// <param name="category">The category of items to filter by.</param>
        /// <param name="condition">The condition of items to filter by (e.g., new, used, like new).</param>
        /// <param name="minPrice">The minimum price for the search results.</param>
        /// <param name="maxPrice">The maximum price for the search results.</param>
        /// <returns>A collection of listings matching the search criteria.</returns>
        /// <response code="200">Returns the filtered list of listings</response>
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EndpointName("SearchListings")]
        [EndpointSummary("Searches for listings based on various criteria.")]
        [EndpointDescription("Filters listings based on search terms, category, condition, and price range.")]
        public async Task<ActionResult<IEnumerable<Listing>>> Search(
            [FromQuery] string query,
            [FromQuery] string category,
            [FromQuery] string condition,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice)
        {
            var listings = await _listingService.SearchListingsAsync(query, category, condition, minPrice, maxPrice);
            return Ok(listings);
        }

        /// <summary>
        /// Creates a new listing in the system with optional image uploads.
        /// </summary>
        /// <param name="listing">The listing information to create.</param>
        /// <param name="files">Optional image files to upload with the listing.</param>
        /// <returns>The newly created listing with its generated ID.</returns>
        /// <response code="201">Returns the newly created listing</response>
        /// <response code="400">If the listing data is invalid</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [EndpointName("CreateListing")]
        [EndpointSummary("Creates a new listing in the system with optional image uploads.")]
        [EndpointDescription("Creates a new product listing with optional image files. The listing information must be provided in the request body and files should be sent as form data.")]
        public async Task<ActionResult<Listing>> Create([FromForm] Listing listing, [FromForm] List<IFormFile> files)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdListing = await _listingService.CreateListingAsync(listing, files);
            return CreatedAtAction(nameof(GetById), new { id = createdListing.Id }, createdListing);
        }

        /// <summary>
        /// Updates an existing listing with optional new images.
        /// </summary>
        /// <param name="id">The unique identifier of the listing to update.</param>
        /// <param name="listing">The updated listing information.</param>
        /// <param name="files">Optional new image files to add to the listing.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">If the listing was successfully updated</response>
        /// <response code="400">If the listing data is invalid</response>
        /// <response code="404">If the listing with the specified ID doesn't exist</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EndpointName("UpdateListing")]
        [EndpointSummary("Updates an existing listing with optional new images.")]
        [EndpointDescription("Updates the details of an existing product listing including title, description, price, and optional new images.")]
        public async Task<IActionResult> Update(string id, [FromForm] Listing listing, [FromForm] List<IFormFile> files)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _listingService.UpdateListingAsync(id, listing, files);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        /// <summary>
        /// Deletes a listing from the system.
        /// </summary>
        /// <param name="id">The unique identifier of the listing to delete.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">If the listing was successfully deleted</response>
        /// <response code="404">If the listing with the specified ID doesn't exist</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EndpointName("DeleteListing")]
        [EndpointSummary("Removes a listing from the system.")]
        [EndpointDescription("Permanently deletes a product listing from the database. This operation cannot be undone.")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _listingService.DeleteListingAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        /// <summary>
        /// Updates the status of an existing listing.
        /// </summary>
        /// <param name="id">The unique identifier of the listing to update.</param>
        /// <param name="status">The new status value (active, sold, reserved, or deleted).</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">If the status was successfully updated</response>
        /// <response code="400">If the status value is invalid</response>
        /// <response code="404">If the listing with the specified ID doesn't exist</response>
        [HttpPatch("{id}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EndpointName("UpdateListingStatus")]
        [EndpointSummary("Updates the status of a listing.")]
        [EndpointDescription("Updates the status of an existing listing. Valid status values include: active, sold, reserved, deleted.")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] string status)
        {
            var validStatuses = new[] { "active", "sold", "reserved", "deleted" };
            if (!validStatuses.Contains(status))
            {
                return BadRequest($"Status must be one of: {string.Join(", ", validStatuses)}");
            }

            var success = await _listingService.UpdateListingStatusAsync(id, status);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
