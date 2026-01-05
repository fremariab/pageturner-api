using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PageTurner.Api.Models;
using PageTurner.Api.Models.DTOs;
using PageTurner.Api.Services.Interfaces;

namespace PageTurner.Api.Controllers
{
    [ApiController]
    [Route("api/v1/books")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        // private readonly ICacheService _cacheService;

        public BookController(IBookService bookService, ICacheService cacheService)
        {
            _bookService = bookService;
            // _cacheService = cacheService;
        }

        // GET /api/v1/books → Get paginated list
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery] string? bookTitle = null,
            [FromQuery] string? authorName = null,
            [FromQuery] string? genre = null
        )
        {
            var cacheKey =
                $"books:page:{page}:limit:{limit}:bookTitle:{bookTitle ?? "all"}:authorName:{authorName ?? "all"}:genre:{genre ?? "all"}";

            // var cachedBooks = await _cacheService.GetAsync<PagedResponse<BookResponse>>(cacheKey);
            // if (cachedBooks != null)
            //     return Ok(cachedBooks);

            var books = await _bookService.GetAllBooksAsync(
                page,
                limit,
                genre,
                bookTitle,
                authorName
            );

            // await _cacheService.SetAsync(cacheKey, books, TimeSpan.FromMinutes(10));

            return Ok(books);
        }

        // GET /api/v1/books/{id} → Get single book by ID
        [HttpGet("{bookId}")]
        public async Task<IActionResult> GetById(string bookId)
        {
            var cacheKey = $"book:{bookId}";

            // 1️⃣ Check cache
            // var cachedBook = await _cacheService.GetAsync<BookResponse>(cacheKey);
            // if (cachedBook != null)
            //     return Ok(cachedBook);

            // 2️⃣ Fetch from DB
            var book = await _bookService.GetBookByIdAsync(bookId);
            if (book == null)
                return NotFound();

            // 3️⃣ Store in cache
            // await _cacheService.SetAsync(cacheKey, book, TimeSpan.FromMinutes(10));

            return Ok(book);
        }

        // POST /api/v1/books → Add a new book
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookRequest request)
        {
            var createdBook = await _bookService.AddBookAsync(request);

            // Invalidate relevant caches
            // await _cacheService.RemoveAsync($"books:page:1:limit:10"); // example for first page
            // Optional: Remove other page caches if you track them

            return CreatedAtAction(
                nameof(GetById),
                new { bookId = createdBook.BookId },
                createdBook
            );
        }

        // PATCH /api/v1/books/{id} → Update a book
        [HttpPut("{bookId}")]
        public async Task<IActionResult> Update(string bookId, [FromBody] BookRequest request)
        {
            var updatedBook = await _bookService.UpdateBookAsync(bookId, request);
            if (updatedBook == null)
                return NotFound();

            // Invalidate caches
            // await _cacheService.RemoveAsync($"book:{bookId}");
            // await _cacheService.RemoveAsync($"books:page:1:limit:10"); // example for first page

            return Ok(updatedBook);
        }

        // DELETE /api/v1/books/{id} → Delete a book
        [HttpDelete("{bookId}")]
        public async Task<IActionResult> Delete(string bookId)
        {
            var deleted = await _bookService.DeleteBookAsync(bookId);
            if (!deleted)
                return NotFound();

            // Invalidate caches
            // await _cacheService.RemoveAsync($"book:{bookId}");
            // await _cacheService.RemoveAsync($"books:page:1:limit:10");

            return NoContent();
        }
    }
}
