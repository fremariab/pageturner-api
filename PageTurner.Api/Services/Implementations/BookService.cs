using System;
using Microsoft.EntityFrameworkCore;
using PageTurner.Api.Data;
using PageTurner.Api.Models.DTOs;
using PageTurner.Api.Models.Entities;
using PageTurner.Api.Models.Filters;
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
            BookFilter? filter = null
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
                    .Books.Join(
                        _context.Authors,
                        book => book.Author,
                        author => author.AuthorId,
                        (book, author) => new { Book = book, Author = author }
                    )
                    .AsQueryable();

                if (filter != null)
                {
                    // Apply filters
                    if (!string.IsNullOrEmpty(filter.BookTitle))
                    {
                        query = query.Where(b => b.Book.BookTitle.Contains(filter.BookTitle));
                    }
                    if (!string.IsNullOrEmpty(filter.Author))
                    {
                        query = query.Where(b => b.Author.AuthorName.Contains(filter.Author));
                    }
                    if (!string.IsNullOrEmpty(filter.ISBN))
                    {
                        query = query.Where(b => b.Book.ISBN.Contains(filter.ISBN));
                    }
                    if (filter.StockQuantity.HasValue)
                    {
                        query = query.Where(b =>
                            b.Book.StockQuantity >= filter.StockQuantity.Value
                        );
                    }

                    if (filter.Price.HasValue)
                    {
                        query = query.Where(b => b.Book.Price <= filter.Price.Value);
                    }
                    if (!string.IsNullOrEmpty(filter.Genre))
                    {
                        query = query.Where(b => b.Book.Genre.Contains(filter.Genre));
                    }

                    if (filter.InStock.HasValue && filter.InStock.Value)
                    {
                        query = query.Where(b => b.Book.StockQuantity > 0);
                    }

                    if (filter.MinPrice.HasValue && filter.MaxPrice.HasValue)
                    {
                        query = query.Where(b =>
                            b.Book.Price >= filter.MinPrice.Value
                            && b.Book.Price <= filter.MaxPrice.Value
                        );
                    }
                    else if (filter.MinPrice.HasValue)
                    {
                        query = query.Where(b => b.Book.Price >= filter.MinPrice.Value);
                    }
                    else if (filter.MaxPrice.HasValue)
                    {
                        query = query.Where(b => b.Book.Price <= filter.MaxPrice.Value);
                    }
                }

                if (!string.IsNullOrWhiteSpace(filter?.SortBy))
                {
                    var isDesc = filter.SortDirection?.ToLower() == "desc";

                    query = filter.SortBy.ToLower() switch
                    {
                        "price" => isDesc
                            ? query.OrderByDescending(b => b.Book.Price)
                            : query.OrderBy(b => b.Book.Price),

                        "bookid" => isDesc
                            ? query.OrderByDescending(b => b.Book.BookId)
                            : query.OrderBy(b => b.Book.BookId),
                        "booktitle" => isDesc
                            ? query.OrderByDescending(b => b.Book.BookTitle)
                            : query.OrderBy(b => b.Book.BookTitle),

                        "averagerating" => isDesc
                            ? query.OrderByDescending(b => b.Book.AverageRating)
                            : query.OrderBy(b => b.Book.AverageRating),
                        "stockquantity" => isDesc
                            ? query.OrderByDescending(b => b.Book.StockQuantity)
                            : query.OrderBy(b => b.Book.StockQuantity),
                        "isbn" => isDesc
                            ? query.OrderByDescending(b => b.Book.ISBN)
                            : query.OrderBy(b => b.Book.ISBN),
                        "author" => isDesc
                            ? query.OrderByDescending(a => a.Author.AuthorName)
                            : query.OrderBy(a => a.Author.AuthorName),

                        _ => query.OrderBy(b => b.Book.BookTitle), // default sort
                    };
                }

                var totalItems = await query.CountAsync();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                var items = await query
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
            catch (Exception e)
            {
                throw new Exception("An error occurred while retrieving organizations =>", e);
            }
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

        public async Task<bool> UpdateBookStockAsync(string bookId, int newStock)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                return false; // Book not found
            }

            book.StockQuantity = newStock;
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> BulkUpdateBookStockAsync(Dictionary<string, int> bookStockUpdates)
        {
            var bookIds = bookStockUpdates.Keys;
            var books = await _context.Books.Where(b => bookIds.Contains(b.BookId)).ToListAsync();

            foreach (var book in books)
            {
                if (bookStockUpdates.TryGetValue(book.BookId, out var newStock))
                {
                    book.StockQuantity = newStock;
                }
            }

            _context.Books.UpdateRange(books);
            await _context.SaveChangesAsync();
            return true;
        }

        // public Task<object> GetAllAsync(int page, int limit)
        // {
        //     return GetAllBooksAsync(page, limit, null).ContinueWith(t => (object)t.Result);
        // }
    }
}
