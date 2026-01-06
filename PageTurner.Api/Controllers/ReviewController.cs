using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PageTurner.Api.Models;
using PageTurner.Api.Models.DTOs;
using PageTurner.Api.Models.Filters;
using PageTurner.Api.Services.Interfaces;

namespace PageTurner.Api.Controllers
{
    [ApiController]
    [Route("api/v1/reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // GET /api/v1/reviews → Get paginated list
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] ReviewFilter? filter,
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10
        )
        {
            var reviews = await _reviewService.GetAllReviewsAsync(page, limit, filter);

            return Ok(reviews);
        }

        // GET /api/v1/reviews/{id} → Get single review by ID
        [HttpGet("{reviewId}")]
        public async Task<IActionResult> GetById(string reviewId)
        {
            var review = await _reviewService.GetReviewByIdAsync(reviewId);
            if (review == null)
                return NotFound();

            return Ok(review);
        }

        // POST /api/v1/reviews → Add a new review
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReviewRequest request)
        {
            var createdReview = await _reviewService.AddReviewAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { reviewId = createdReview.ReviewId },
                createdReview
            );
        }

        // POST /api/v1/reviews/bulk → Add multiple reviews
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkCreate([FromBody] List<ReviewRequest> requests)
        {
            if (requests == null || !requests.Any())
            {
                return BadRequest("Request body cannot be empty.");
            }

            var invalidRequests = new List<string>();
            var createdReviews = new List<ReviewResponse>();

            foreach (var request in requests)
            {
                try
                {
                    var createdReview = await _reviewService.AddReviewAsync(request);
                    createdReviews.Add(createdReview);
                }
                catch (KeyNotFoundException ex)
                {
                    invalidRequests.Add($"Review for BookId {request.BookId} failed: {ex.Message}");
                }
                catch (Exception ex)
                {
                    invalidRequests.Add($"Review for BookId {request.BookId} failed: {ex.Message}");
                }
            }

            if (invalidRequests.Any())
            {
                return BadRequest(
                    new { Message = "Some reviews could not be created.", Errors = invalidRequests }
                );
            }

            return Ok(createdReviews);
        }

        // DELETE /api/v1/reviews/{id} → Delete a review
        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> Delete(string reviewId)
        {
            var deleted = await _reviewService.DeleteReviewAsync(reviewId);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpGet("{reviewId}/books")]
        public async Task<IActionResult> GetReviewsByBook(
            [FromRoute] string bookId,
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10
        )
        {
            var books = await _reviewService.GetReviewByBookIdAsync(page, limit, bookId);
            if (books == null)
                return NotFound();

            return Ok(books);
        }
    }
}
