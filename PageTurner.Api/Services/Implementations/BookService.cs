using System;
using Microsoft.EntityFrameworkCore;
using PageTurner.Api.Data;
using PageTurner.Api.Models.DTOs;
using PageTurner.Api.Models.Entities;
using PageTurner.Api.Services.Interfaces;

namespace PageTurner.Api.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly PageTurnerDbContext _context;

        public BookService(PageTurnerDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResponse<BookResponse>> GetAllBooksAsync(
            int pageNumber,
            int pageSize,
            string? genre,
            string? bookTitle,
            string? author
        )
        {
            var query = _context.Books.AsNoTracking();
            if (!string.IsNullOrEmpty(genre))
                query = query.Where(b => b.Genre == genre);


            if (!string.IsNullOrEmpty(bookTitle))
                query = query.Where(b => b.BookTitle.Contains(bookTitle));

            var join_query = query
                .Join(
                    _context.Authors,
                    book => book.Author,
                    author => author.AuthorId,
                    (book, author) => new { Book = book, Author = author }
                )
                .AsQueryable();

            if (!string.IsNullOrEmpty(author))
                join_query = join_query.Where(b => b.Author.AuthorName.Contains(author));

            var totalItems = await join_query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var items = await join_query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new BookResponse
                {
                    BookId = x.Book.BookId,
                    BookTitle = x.Book.BookTitle,
                    Author = x.Author != null ? x.Author.AuthorName : x.Book.Author,
                    ISBN = x.Book.ISBN,
                    Price = x.Book.Price,
                    StockQuantity = x.Book.StockQuantity,
                    Genre = x.Book.Genre,
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

        public async Task<BookResponse?> GetBookByIdAsync(string bookId)
        {
            var book = await _context.Books.FindAsync(bookId);

            if (book == null)
                return null;

            return new BookResponse
            {
                BookId = book.BookId,
                BookTitle = book.BookTitle,
                Author = book.Author,
                ISBN = book.ISBN,
                Price = book.Price,
                StockQuantity = book.StockQuantity,
                Genre = book.Genre,
            };
        }

        public async Task<BookResponse> AddBookAsync(BookRequest request)
        {
            var book = new Book
            {
                BookId = Guid.NewGuid().ToString(),
                BookTitle = request.BookTitle,
                Author = request.Author,
                ISBN = request.ISBN,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                Genre = request.Genre,
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return new BookResponse
            {
                BookId = book.BookId,
                BookTitle = book.BookTitle,
                Author = book.Author,
                ISBN = book.ISBN,
                Price = book.Price,
                StockQuantity = book.StockQuantity,
                Genre = book.Genre,
            };
        }

        public async Task<BookResponse?> UpdateBookAsync(string bookId, BookRequest request)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
                return null;

            book.BookTitle = request.BookTitle ?? book.BookTitle;
            book.Author = request.Author ?? book.Author;
            book.ISBN = request.ISBN ?? book.ISBN;
            book.Price = request.Price;
            book.StockQuantity = request.StockQuantity;
            book.Genre = request.Genre ?? book.Genre;

            await _context.SaveChangesAsync();

            return new BookResponse
            {
                BookId = book.BookId,
                BookTitle = book.BookTitle,
                Author = book.Author,
                ISBN = book.ISBN,
                Price = book.Price,
                StockQuantity = book.StockQuantity,
                Genre = book.Genre,
            };
        }

        public async Task<bool> DeleteBookAsync(string bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
                return false;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }

        // public Task<object> GetAllAsync(int page, int limit)
        // {
        //     return GetAllBooksAsync(page, limit, null).ContinueWith(t => (object)t.Result);
        // }
    }
}
