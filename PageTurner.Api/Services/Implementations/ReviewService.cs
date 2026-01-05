using Microsoft.EntityFrameworkCore;
using PageTurner.Api.Data;
using PageTurner.Api.Models.DTOs;
using PageTurner.Api.Models.Entities;
using PageTurner.Api.Services.Interfaces;

namespace PageTurner.Api.Services.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly PageTurnerDbContext _context;

        public ReviewService(PageTurnerDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResponse<ReviewResponse>> GetAllReviewsAsync(
            int pageNumber,
            int pageSize,
            string? reviewId,
            string? reviewerName
        )
        {
            var query = _context.Reviews.AsQueryable();

            if (!string.IsNullOrEmpty(reviewId))
                query = query.Where(r => r.ReviewId == reviewId);

            if (!string.IsNullOrEmpty(reviewerName))
                query = query.Where(r => r.ReviewerName == reviewerName);

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var items = await query
                .Join(
                    _context.Books,
                    review => review.BookId,
                    book => book.BookId,
                    (review, book) =>
                        new ReviewResponse
                        {
                            ReviewId = review.ReviewId,
                            ReviewerName = review.ReviewerName,
                            Rating = review.Rating,
                            Comment = review.Comment,
                            BookId = review.BookId,
                            BookTitle = book.BookTitle,
                            Author = book.Author,
                        }
                )
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResponse<ReviewResponse>
            {
                Data = items,
                Pagination = new Pagination
                {
                    TotalItems = totalItems,
                    CurrentPage = pageNumber,
                    TotalPages = totalPages,
                    Limit = pageSize,
                },
            };
        }

        public async Task<ReviewResponse?> GetReviewByIdAsync(string reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);

            if (review == null)
                return null;

            return new ReviewResponse
            {
                ReviewId = review.ReviewId,
                ReviewerName = review.ReviewerName,
                Rating = review.Rating,
                Comment = review.Comment,
            };
        }

        public async Task<PagedResponse<ReviewResponse>> GetReviewByBookIdAsync(
            int pageNumber,
            int pageSize,
            string bookId
        )
        {
            var query = _context.Reviews.AsQueryable();

            if (!string.IsNullOrEmpty(bookId))
                query = query.Where(r => r.BookId == bookId);

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var items = await query
                .GroupJoin(
                    _context.Reviews,
                    b => b.BookId,
                    r => r.BookId,
                    (reviews, books) => new { Review = reviews, Book = books.FirstOrDefault() }
                )
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(review => new ReviewResponse
                {
                    ReviewId = review.Review.ReviewId,
                    ReviewerName = review.Review.ReviewerName,
                    Rating = review.Review.Rating,
                    Comment = review.Review.Comment,
                })
                .ToListAsync();

            return new PagedResponse<ReviewResponse>
            {
                Data = items,
                Pagination = new Pagination
                {
                    TotalItems = totalItems,
                    CurrentPage = pageNumber,
                    TotalPages = totalPages,
                    Limit = pageSize,
                },
            };
        }

        public async Task<ReviewResponse> AddReviewAsync(ReviewRequest request)
        {
            var book = await _context.Books.FindAsync(request.BookId);
            if (book == null)
            {
                throw new Exception("Book not found.");
            }

            var review = new Review
            {
                ReviewId = Guid.NewGuid().ToString(),
                ReviewerName = request.ReviewerName,
                Comment = request.Comment ?? string.Empty,
                Rating = request.Rating,
                BookId = request.BookId,
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return new ReviewResponse
            {
                ReviewId = Guid.NewGuid().ToString(),
                ReviewerName = review.ReviewerName,
                Comment = review.Comment ?? string.Empty,
                Rating = review.Rating,
                BookId = review.BookId,
                BookTitle = book.BookTitle,
                Author = book.Author,
            };
        }

        public async Task<bool> DeleteReviewAsync(string bookId)
        {
            var review = await _context.Reviews.FindAsync(bookId);
            if (review == null)
                return false;

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
