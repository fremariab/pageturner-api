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
    [Route("api/v1/authors")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        // private readonly ICacheService _cacheService;

        // public AuthorController(IAuthorService authorService, ICacheService cacheService)

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
            // _cacheService = cacheService;
        }

        // GET /api/v1/authors → Get paginated list
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery] string? authorId = null,
            [FromQuery] string? authorName = null
        )
        {
            var cacheKey = $"authors:page:{page}:limit:{limit}";

            // 1️⃣ Check cache
            // var cachedAuthors = await _cacheService.GetAsync<PagedResponse<AuthorResponse>>(
            //     cacheKey
            // );
            // if (cachedAuthors != null)
            //     return Ok(cachedAuthors);

            // 2️⃣ Fetch from DB
            var authors = await _authorService.GetAllAuthorsAsync(
                page,
                limit,
                authorId,
                authorName
            );

            // 3️⃣ Store in cache
            // await _cacheService.SetAsync(cacheKey, authors, TimeSpan.FromMinutes(10));

            return Ok(authors);
        }

        // GET /api/v1/authors/{id} → Get single author by ID
        [HttpGet("{authorId}")]
        public async Task<IActionResult> GetById(string authorId)
        {
            var cacheKey = $"author:{authorId}";

            // 1️⃣ Check cache
            // var cachedAuthor = await _cacheService.GetAsync<AuthorResponse>(cacheKey);
            // if (cachedAuthor != null)
            //     return Ok(cachedAuthor);

            // 2️⃣ Fetch from DB
            var author = await _authorService.GetAuthorByIdAsync(authorId);
            if (author == null)
                return NotFound();

            // 3️⃣ Store in cache
            // await _cacheService.SetAsync(cacheKey, author, TimeSpan.FromMinutes(10));

            return Ok(author);
        }

        // POST /api/v1/authors → Add a new author
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AuthorRequest request)
        {
            var createdAuthor = await _authorService.AddAuthorAsync(request);

            // Invalidate relevant caches
            // await _cacheService.RemoveAsync($"authors:page:1:limit:10"); // example for first page
            // Optional: Remove other page caches if you track them

            return CreatedAtAction(
                nameof(GetById),
                new { authorId = createdAuthor.AuthorId },
                createdAuthor
            );
        }

        // PATCH /api/v1/authors/{id} → Update a author
        [HttpPut("{authorId}")]
        public async Task<IActionResult> Update(string authorId, [FromBody] AuthorRequest request)
        {
            var updatedAuthor = await _authorService.UpdateAuthorAsync(authorId, request);
            if (updatedAuthor == null)
                return NotFound();

            // Invalidate caches
            // await _cacheService.RemoveAsync($"author:{authorId}");
            // await _cacheService.RemoveAsync($"authors:page:1:limit:10"); // example for first page

            return Ok(updatedAuthor);
        }

        // DELETE /api/v1/authors/{id} → Delete a author
        [HttpDelete("{authorId}")]
        public async Task<IActionResult> Delete(string authorId)
        {
            var deleted = await _authorService.DeleteAuthorAsync(authorId);
            if (!deleted)
                return NotFound();

            // Invalidate caches
            // await _cacheService.RemoveAsync($"author:{authorId}");
            // await _cacheService.RemoveAsync($"authors:page:1:limit:10");

            return NoContent();
        }

        [HttpGet("{authorId}/books")]
        public async Task<IActionResult> GetBooksByAuthor(
            [FromRoute] string authorId,
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10
        )
        {
            var cacheKey = $"author:{authorId}:books:page:{page}:limit:{limit}";

            // 1️⃣ Check cache
            // var cachedBooks = await _cacheService.GetAsync<PagedResponse<BookResponse>>(cacheKey);
            // if (cachedBooks != null)
            //     return Ok(cachedBooks);

            // 2️⃣ Fetch from DB
            var books = await _authorService.GetAllBooksByAuthorIdAsync(page, limit, authorId);
            if (books == null)
                return NotFound();

            // 3️⃣ Store in cache
            // await _cacheService.SetAsync(cacheKey, books, TimeSpan.FromMinutes(10));

            return Ok(books);
        }
    }
}
