using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PageTurner.Api.Models;
using PageTurner.Api.Models.DTOs;
using PageTurner.Api.Models.Filters;
using PageTurner.Api.Services.Interfaces;

namespace PageTurner.Api.Controllers
{
    [ApiController]
    [Route("api/v1/books")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET /api/v1/books → Get paginated list
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] BookFilter? filter,
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10
        )
        {
            var books = await _bookService.GetAllBooksAsync(page, limit, filter);

            return Ok(books);
        }

        // GET /api/v1/books/{id} → Get single book by ID
        [HttpGet("{bookId}")]
        public async Task<IActionResult> GetById(string bookId)
        {
            // 2️⃣ Fetch from DB
            var book = await _bookService.GetBookByIdAsync(bookId);
            if (book == null)
                return NotFound();

            return Ok(book);
        }

        // POST /api/v1/books → Add a new book
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookRequest request)
        {
            var createdBook = await _bookService.AddBookAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { bookId = createdBook.BookId },
                createdBook
            );
        }

        // POST /api/v1/books/bulk → Add multiple books
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkCreate([FromBody] List<BookRequest> requests)
        {
            if (requests == null || !requests.Any())
            {
                return BadRequest("Request body cannot be empty.");
            }

            var invalidRequests = new List<string>();
            var createdBooks = new List<BookResponse>();

            foreach (var request in requests)
            {
                try
                {
                    var createdBook = await _bookService.AddBookAsync(request);
                    createdBooks.Add(createdBook);
                }
                catch (Exception ex)
                {
                    invalidRequests.Add(
                        $"Book with Title {request.BookTitle} failed: {ex.Message}"
                    );
                }
            }

            if (invalidRequests.Any())
            {
                return BadRequest(
                    new { Message = "Some books could not be created.", Errors = invalidRequests }
                );
            }

            return Ok(createdBooks);
        }

        // PATCH /api/v1/books/{id} → Update a book
        [HttpPut("{bookId}")]
        public async Task<IActionResult> Update(string bookId, [FromBody] BookRequest request)
        {
            var updatedBook = await _bookService.UpdateBookAsync(bookId, request);
            if (updatedBook == null)
                return NotFound();

            return Ok(updatedBook);
        }

        // DELETE /api/v1/books/{id} → Delete a book
        [HttpDelete("{bookId}")]
        public async Task<IActionResult> Delete(string bookId)
        {
            var deleted = await _bookService.DeleteBookAsync(bookId);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
