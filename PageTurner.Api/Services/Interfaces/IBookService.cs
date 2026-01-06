using PageTurner.Api.Models.DTOs;
using PageTurner.Api.Models.Filters;

namespace PageTurner.Api.Services.Interfaces
{
    public interface IBookService
    {
        Task<PagedResponse<BookResponse>> GetAllBooksAsync(
            int pageNumber,
            int pageSize,
            BookFilter? filter = null
        );
        Task<BookResponse?> GetBookByIdAsync(string bookId);
        Task<BookResponse> AddBookAsync(BookRequest request);
        Task<BookResponse?> UpdateBookAsync(string bookId, BookRequest request);
        Task<bool> DeleteBookAsync(string bookId);
    }
}
