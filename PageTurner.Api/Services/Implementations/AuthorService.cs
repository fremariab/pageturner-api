using Microsoft.EntityFrameworkCore;
using PageTurner.Api.Data;
using PageTurner.Api.Models.DTOs;
using PageTurner.Api.Models.Entities;
using PageTurner.Api.Services.Interfaces;

namespace PageTurner.Api.Services.Implementations
{
    public class AuthorService : IAuthorService
    {
        private readonly PageTurnerDbContext _context;

        public AuthorService(PageTurnerDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResponse<AuthorResponse>> GetAllAuthorsAsync(
            int pageNumber,
            int pageSize,
            string? authorName,
            string? authorId
        )
        {
            var query = _context.Authors.AsQueryable();

            if (!string.IsNullOrEmpty(authorId))
                query = query.Where(a => a.AuthorId == authorId);

            if (!string.IsNullOrEmpty(authorName))
                query = query.Where(a => a.AuthorName == authorName);

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AuthorResponse
                {
                    AuthorId = a.AuthorId,
                    AuthorName = a.AuthorName,
                    AuthorBio = a.AuthorBio ?? string.Empty,
                })
                .ToListAsync();

            return new PagedResponse<AuthorResponse>
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

        public async Task<AuthorResponse?> GetAuthorByIdAsync(string authorId)
        {
            var author = await _context.Authors.FindAsync(authorId);

            if (author == null)
                return null;

            return new AuthorResponse
            {
                AuthorId = author.AuthorId,
                AuthorName = author.AuthorName,
                AuthorBio = author.AuthorBio ?? string.Empty,
            };
        }

        public async Task<AuthorResponse> AddAuthorAsync(AuthorRequest request)
        {
            var author = new Author
            {
                AuthorId = Guid.NewGuid().ToString(),
                AuthorName = request.AuthorName,
                AuthorBio = request.AuthorBio ?? string.Empty,
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return new AuthorResponse
            {
                AuthorId = author.AuthorId,
                AuthorName = author.AuthorName,
                AuthorBio = author.AuthorBio ?? string.Empty,
            };
        }

        public async Task<AuthorResponse?> UpdateAuthorAsync(string authorId, AuthorRequest request)
        {
            var author = await _context.Authors.FindAsync(authorId);
            if (author == null)
                return null;

            author.AuthorName = request.AuthorName ?? author.AuthorName;
            author.AuthorBio = request.AuthorBio ?? author.AuthorBio;

            await _context.SaveChangesAsync();

            return new AuthorResponse
            {
                AuthorName = author.AuthorName,
                AuthorBio = author.AuthorBio ?? string.Empty,
            };
        }

        public async Task<bool> DeleteAuthorAsync(string authorId)
        {
            var author = await _context.Authors.FindAsync(authorId);
            if (author == null)
                return false;

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResponse<BookResponse>> GetAllBooksByAuthorIdAsync(
            int pageNumber,
            int pageSize,
            string authorId
        )
        {
            var query = _context.Books.AsQueryable();

            if (!string.IsNullOrEmpty(authorId))
                query = query.Where(b => b.Author == authorId);

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BookResponse
                {
                    BookId = b.BookId,
                    BookTitle = b.BookTitle,
                    Author = b.Author,
                    ISBN = b.ISBN,
                    Price = b.Price,
                    StockQuantity = b.StockQuantity,
                    Genre = b.Genre,
                })
                .ToListAsync();

            return new PagedResponse<BookResponse>
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
    }
}
