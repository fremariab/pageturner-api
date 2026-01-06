using Microsoft.EntityFrameworkCore;
using PageTurner.Api.Data;
using PageTurner.Api.Models.DTOs;
using PageTurner.Api.Models.Entities;
using PageTurner.Api.Models.Filters;
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
            ReviewFilter? filter = null
        )
        {
            try
            {
                if (pageNumber <= 0)
                {
                    throw new ArgumentException("Page number must be greater than 0");
                }
                if (pageSize <= 0 || pageSize > 100)
                {
                    throw new ArgumentException("Page size must be between 1 and 100");
                }

                var query = _context
                    .Reviews.Join(
                        _context.Books,
                        review => review.BookId,
                        book => book.BookId,
                        (review, book) => new { Review = review, Book = book }
                    )
                    .AsQueryable();

                if (filter != null)
                {
                    if (!string.IsNullOrEmpty(filter.ReviewerName))
                        query = query.Where(r =>
                            r.Review.ReviewerName.Contains(filter.ReviewerName)
                        );

                    if (!string.IsNullOrEmpty(filter.Comment))
                    {
                        query = query.Where(r =>
                            r.Review.Comment != null && r.Review.Comment.Contains(filter.Comment)
                        );
                    }
                    if (!string.IsNullOrEmpty(filter.BookTitle))
                    {
                        query = query.Where(r => r.Book.BookTitle.Contains(filter.BookTitle));
                    }
                    if (filter.Rating.HasValue)
                    {
                        query = query.Where(r => r.Review.Rating >= filter.Rating.Value);
                    }
                }

                var totalItems = await query.CountAsync();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                var items = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => new ReviewResponse
                    {
                        ReviewId = x.Review.ReviewId,
                        ReviewerName = x.Review.ReviewerName,
                        Rating = x.Review.Rating,
                        Comment = x.Review.Comment,
                        BookId = x.Review.BookId,
                        BookTitle = x.Book.BookTitle,
                        Author = x.Book.Author,
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
            catch (Exception)
            {
                throw;
            }
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
                throw new KeyNotFoundException("Book not found.");
            }

            var reviewId = Guid.NewGuid().ToString();
            var review = new Review
            {
                ReviewId = reviewId,
                ReviewerName = request.ReviewerName,
                Comment = request.Comment ?? string.Empty,
                Rating = request.Rating,
                BookId = request.BookId,
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return new ReviewResponse
            {
                ReviewId = reviewId,
                ReviewerName = review.ReviewerName,
                Comment = review.Comment,
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
            {
                throw new KeyNotFoundException("Review not found.");
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
