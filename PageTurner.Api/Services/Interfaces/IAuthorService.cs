using PageTurner.Api.Models.DTOs;
using PageTurner.Api.Models.Filters;

namespace PageTurner.Api.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<PagedResponse<AuthorResponse>> GetAllAuthorsAsync(
            int pageNumber,
            int pageSize,
            AuthorFilter? filter = null
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
