using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PageTurner.Api.Models;
using PageTurner.Api.Models.DTOs;
using PageTurner.Api.Services.Interfaces;

namespace PageTurner.Api.Controllers
{
    [ApiController]
    [Route("api/v1/reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        // private readonly ICacheService _cacheService;

        public ReviewController(IReviewService reviewService)
        // public ReviewController(IReviewService reviewService, ICacheService cacheService)
        {
            _reviewService = reviewService;
            // _cacheService = cacheService;
        }

        // GET /api/v1/reviews → Get paginated list
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery] string? reviewId = null,
            [FromQuery] string? reviewerName = null
        )
        {
            var cacheKey = $"reviews:page:{page}:limit:{limit}";

            // var cachedReviews = await _cacheService.GetAsync<PagedResponse<ReviewResponse>>(
            //     cacheKey
            // );
            // if (cachedReviews != null)
            //     return Ok(cachedReviews);

            var reviews = await _reviewService.GetAllReviewsAsync(
                page,
                limit,
                reviewId,
                reviewerName
            );

            // await _cacheService.SetAsync(cacheKey, reviews, TimeSpan.FromMinutes(10));

            return Ok(reviews);
        }

        // GET /api/v1/reviews/{id} → Get single review by ID
        [HttpGet("{reviewId}")]
        public async Task<IActionResult> GetById(string reviewId)
        {
            var cacheKey = $"review:{reviewId}";

            // 1️⃣ Check cache
            // var cachedReview = await _cacheService.GetAsync<ReviewResponse>(cacheKey);
            // if (cachedReview != null)
            //     return Ok(cachedReview);

            // 2️⃣ Fetch from DB
            var review = await _reviewService.GetReviewByIdAsync(reviewId);
            if (review == null)
                return NotFound();

            // 3️⃣ Store in cache
            // await _cacheService.SetAsync(cacheKey, review, TimeSpan.FromMinutes(10));

            return Ok(review);
        }

        // POST /api/v1/reviews → Add a new review
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReviewRequest request)
        {
            var createdReview = await _reviewService.AddReviewAsync(request);

            // Invalidate relevant caches
            // await _cacheService.RemoveAsync($"reviews:page:1:limit:10"); // example for first page
            // Optional: Remove other page caches if you track them

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

            var createdReviews = new List<ReviewResponse>();

            foreach (var request in requests)
            {
                var createdReview = await _reviewService.AddReviewAsync(request);
                createdReviews.Add(createdReview);
            }

            // Optional: Invalidate relevant caches
            // await _cacheService.RemoveAsync("reviews:page:1:limit:10");

            return Ok(createdReviews);
        }

        // DELETE /api/v1/reviews/{id} → Delete a review
        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> Delete(string reviewId)
        {
            var deleted = await _reviewService.DeleteReviewAsync(reviewId);
            if (!deleted)
                return NotFound();

            // Invalidate caches
            // await _cacheService.RemoveAsync($"review:{reviewId}");
            // await _cacheService.RemoveAsync($"reviews:page:1:limit:10");

            return NoContent();
        }

        [HttpGet("{reviewId}/books")]
        public async Task<IActionResult> GetReviewsByBook(
            [FromRoute] string bookId,
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10
        )
        {
            var cacheKey = $"review:{bookId}:books:page:{page}:limit:{limit}";

            // 1️⃣ Check cache
            // var cachedBooks = await _cacheService.GetAsync<PagedResponse<ReviewResponse>>(cacheKey);
            // if (cachedBooks != null)
            //     return Ok(cachedBooks);

            // 2️⃣ Fetch from DB
            var books = await _reviewService.GetReviewByBookIdAsync(page, limit, bookId);
            if (books == null)
                return NotFound();

            // 3️⃣ Store in cache
            // await _cacheService.SetAsync(cacheKey, books, TimeSpan.FromMinutes(10));

            return Ok(books);
        }
    }
}
