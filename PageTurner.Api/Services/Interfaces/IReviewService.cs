using PageTurner.Api.Models.DTOs;
using PageTurner.Api.Models.Entities;
using PageTurner.Api.Models.Filters;

namespace PageTurner.Api.Services.Interfaces
{
    public interface IReviewService
    {
        Task<PagedResponse<ReviewResponse>> GetAllReviewsAsync(
            int pageNumber,
            int pageSize,
            ReviewFilter? filter = null
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
