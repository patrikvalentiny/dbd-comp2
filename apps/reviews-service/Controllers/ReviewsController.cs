using Microsoft.AspNetCore.Mvc;
using ReviewsService.Domain.Models;
using ReviewsService.Services;

namespace ReviewsService.Controllers
{
    /// <summary>
    /// API controller for managing reviews in the second-hand e-commerce platform.
    /// Provides endpoints for creating, retrieving, updating, and deleting reviews,
    /// as well as specialized endpoints for getting reviews by target type (seller, buyer, item).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController(IReviewService reviewService) : ControllerBase
    {
        /// <summary>
        /// Retrieves all reviews in the system.
        /// </summary>
        /// <returns>A collection of all reviews.</returns>
        /// <response code="200">Returns the list of reviews</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EndpointName("GetAllReviews")]
        [EndpointSummary("Retrieves all reviews in the system.")]
        [EndpointDescription("Returns a collection of all reviews with complete details.")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            var reviews = await reviewService.GetAllReviewsAsync();
            return Ok(reviews);
        }

        /// <summary>
        /// Retrieves a specific review by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the review to retrieve.</param>
        /// <returns>The review with the specified ID.</returns>
        /// <response code="200">Returns the requested review</response>
        /// <response code="404">If the review with the specified ID doesn't exist</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EndpointName("GetReviewById")]
        [EndpointSummary("Retrieves a specific review by ID.")]
        [EndpointDescription("Fetches the complete details of a single review specified by its unique identifier.")]
        public async Task<ActionResult<Review>> GetReview(Guid id)
        {
            var review = await reviewService.GetReviewByIdAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return Ok(review);
        }

        /// <summary>
        /// Retrieves all reviews submitted by a specific reviewer.
        /// </summary>
        /// <param name="reviewerId">The unique identifier of the reviewer.</param>
        /// <returns>A collection of reviews submitted by the specified reviewer.</returns>
        /// <response code="200">Returns the list of reviews for the specified reviewer</response>
        [HttpGet("reviewer/{reviewerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EndpointName("GetReviewsByReviewer")]
        [EndpointSummary("Retrieves all reviews submitted by a specific user.")]
        [EndpointDescription("Returns a collection of reviews that were created by the user with the specified ID.")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsByReviewer(Guid reviewerId)
        {
            var reviews = await reviewService.GetReviewsByReviewerIdAsync(reviewerId);
            return Ok(reviews);
        }

        /// <summary>
        /// Retrieves all reviews for a specific seller.
        /// </summary>
        /// <param name="sellerId">The unique identifier of the seller.</param>
        /// <returns>A collection of reviews for the specified seller.</returns>
        /// <response code="200">Returns the list of reviews for the specified seller</response>
        [HttpGet("seller/{sellerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EndpointName("GetReviewsBySeller")]
        [EndpointSummary("Retrieves all reviews for a specific seller.")]
        [EndpointDescription("Returns a collection of reviews that were left for the seller with the specified ID.")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsBySeller(Guid sellerId)
        {
            var reviews = await reviewService.GetReviewsBySellerIdAsync(sellerId);
            return Ok(reviews);
        }

        /// <summary>
        /// Gets the average rating for a specific seller.
        /// </summary>
        /// <param name="sellerId">The unique identifier of the seller.</param>
        /// <returns>The average rating for the specified seller.</returns>
        /// <response code="200">Returns the average rating</response>
        [HttpGet("seller/{sellerId}/rating")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EndpointName("GetSellerRating")]
        [EndpointSummary("Gets the average rating for a seller.")]
        [EndpointDescription("Calculates and returns the average rating from all reviews for the specified seller.")]
        public async Task<ActionResult<double>> GetSellerRating(Guid sellerId)
        {
            var rating = await reviewService.GetSellerRatingAsync(sellerId);
            return Ok(rating);
        }

        /// <summary>
        /// Retrieves all reviews for a specific buyer.
        /// </summary>
        /// <param name="buyerId">The unique identifier of the buyer.</param>
        /// <returns>A collection of reviews for the specified buyer.</returns>
        /// <response code="200">Returns the list of reviews for the specified buyer</response>
        [HttpGet("buyer/{buyerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EndpointName("GetReviewsByBuyer")]
        [EndpointSummary("Retrieves all reviews for a specific buyer.")]
        [EndpointDescription("Returns a collection of reviews that were left for the buyer with the specified ID.")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsByBuyer(Guid buyerId)
        {
            var reviews = await reviewService.GetReviewsByBuyerIdAsync(buyerId);
            return Ok(reviews);
        }

        /// <summary>
        /// Gets the average rating for a specific buyer.
        /// </summary>
        /// <param name="buyerId">The unique identifier of the buyer.</param>
        /// <returns>The average rating for the specified buyer.</returns>
        /// <response code="200">Returns the average rating</response>
        [HttpGet("buyer/{buyerId}/rating")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EndpointName("GetBuyerRating")]
        [EndpointSummary("Gets the average rating for a buyer.")]
        [EndpointDescription("Calculates and returns the average rating from all reviews for the specified buyer.")]
        public async Task<ActionResult<double>> GetBuyerRating(Guid buyerId)
        {
            var rating = await reviewService.GetBuyerRatingAsync(buyerId);
            return Ok(rating);
        }

        /// <summary>
        /// Retrieves all reviews for a specific item.
        /// </summary>
        /// <param name="itemId">The unique identifier of the item.</param>
        /// <returns>A collection of reviews for the specified item.</returns>
        /// <response code="200">Returns the list of reviews for the specified item</response>
        [HttpGet("item/{itemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EndpointName("GetReviewsByItem")]
        [EndpointSummary("Retrieves all reviews for a specific item.")]
        [EndpointDescription("Returns a collection of reviews that were left for the item with the specified ID.")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsByItem(Guid itemId)
        {
            var reviews = await reviewService.GetReviewsByItemIdAsync(itemId);
            return Ok(reviews);
        }

        /// <summary>
        /// Gets the average rating for a specific item.
        /// </summary>
        /// <param name="itemId">The unique identifier of the item.</param>
        /// <returns>The average rating for the specified item.</returns>
        /// <response code="200">Returns the average rating</response>
        [HttpGet("item/{itemId}/rating")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EndpointName("GetItemRating")]
        [EndpointSummary("Gets the average rating for an item.")]
        [EndpointDescription("Calculates and returns the average rating from all reviews for the specified item.")]
        public async Task<ActionResult<double>> GetItemRating(Guid itemId)
        {
            var rating = await reviewService.GetItemRatingAsync(itemId);
            return Ok(rating);
        }

        /// <summary>
        /// Creates a new review in the system.
        /// </summary>
        /// <param name="review">The review information to create.</param>
        /// <returns>The newly created review with its generated ID.</returns>
        /// <response code="201">Returns the newly created review</response>
        /// <response code="400">If the review data is invalid</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [EndpointName("CreateReview")]
        [EndpointSummary("Creates a new review in the system.")]
        [EndpointDescription("Creates a new review. The review information must be provided in the request body.")]
        public async Task<ActionResult<Review>> CreateReview([FromBody] Review review)
        {
            try
            {
                var createdReview = await reviewService.CreateReviewAsync(review);
                return CreatedAtAction(nameof(GetReview), new { id = createdReview.Id }, createdReview);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing review.
        /// </summary>
        /// <param name="id">The unique identifier of the review to update.</param>
        /// <param name="review">The updated review information.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">If the review was successfully updated</response>
        /// <response code="400">If the review data is invalid</response>
        /// <response code="404">If the review with the specified ID doesn't exist</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EndpointName("UpdateReview")]
        [EndpointSummary("Updates an existing review.")]
        [EndpointDescription("Updates the details of an existing review including rating, comment, etc.")]
        public async Task<IActionResult> UpdateReview(Guid id, [FromBody] Review review)
        {
            if (id != review.Id)
            {
                return BadRequest("ID in URL must match ID in request body");
            }

            var existingReview = await reviewService.GetReviewByIdAsync(id);
            if (existingReview == null)
            {
                return NotFound();
            }

            try
            {
                await reviewService.UpdateReviewAsync(review);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a review from the system.
        /// </summary>
        /// <param name="id">The unique identifier of the review to delete.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">If the review was successfully deleted</response>
        /// <response code="404">If the review with the specified ID doesn't exist</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EndpointName("DeleteReview")]
        [EndpointSummary("Removes a review from the system.")]
        [EndpointDescription("Permanently deletes a review from the database. This operation cannot be undone.")]
        public async Task<IActionResult> DeleteReview(Guid id)
        {
            var review = await reviewService.GetReviewByIdAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            await reviewService.DeleteReviewAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Marks a review as helpful, incrementing its helpful count.
        /// </summary>
        /// <param name="id">The unique identifier of the review to mark as helpful.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">If the review was successfully marked as helpful</response>
        /// <response code="404">If the review with the specified ID doesn't exist</response>
        [HttpPost("{id}/helpful")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EndpointName("MarkReviewAsHelpful")]
        [EndpointSummary("Marks a review as helpful.")]
        [EndpointDescription("Increments the helpful count for a review, indicating that a user found the review useful.")]
        public async Task<IActionResult> MarkAsHelpful(Guid id)
        {
            var review = await reviewService.GetReviewByIdAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            await reviewService.MarkReviewAsHelpfulAsync(id);
            return NoContent();
        }
    }
}
