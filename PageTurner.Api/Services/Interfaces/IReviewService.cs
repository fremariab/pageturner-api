using PageTurner.Api.Models.DTOs;

namespace PageTurner.Api.Services.Interfaces
{
    public interface IReviewService
    {
        Task<PagedResponse<ReviewResponse>> GetAllReviewsAsync(
            int pageNumber,
            int pageSize,
            string? reviewId,
            string? reviewerName
        );
        Task<ReviewResponse?> GetReviewByIdAsync(string reviewId);
        Task<PagedResponse<ReviewResponse>> GetReviewByBookIdAsync(
            int pageNumber,
            int pageSize,
            string bookId
        );
        Task<ReviewResponse> AddReviewAsync(ReviewRequest request);
        Task<bool> DeleteReviewAsync(string bookId);
    }
}
