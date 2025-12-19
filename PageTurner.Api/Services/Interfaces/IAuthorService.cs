using PageTurner.Api.Models.DTOs;

namespace PageTurner.Api.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<PagedResponse<AuthorResponse>> GetAllAuthorsAsync(
            int pageNumber,
            int pageSize,
            string? authorName,
            string? authorId
        );
        Task<AuthorResponse?> GetAuthorByIdAsync(string bookId);
        Task<AuthorResponse> AddAuthorAsync(AuthorRequest request);
        Task<AuthorResponse?> UpdateAuthorAsync(string bookId, AuthorRequest request);
        Task<bool> DeleteAuthorAsync(string bookId);
        Task<PagedResponse<BookResponse>> GetAllBooksByAuthorIdAsync(
            int pageNumber,
            int pageSize,
            string authorId
        );
    }
}
