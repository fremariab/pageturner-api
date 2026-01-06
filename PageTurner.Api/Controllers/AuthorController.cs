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
    [Route("api/v1/authors")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        // GET /api/v1/authors → Get paginated list
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] AuthorFilter? filter,
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10
        )
        {
            var authors = await _authorService.GetAllAuthorsAsync(page, limit, filter);

            return Ok(authors);
        }

        // GET /api/v1/authors/{id} → Get single author by ID
        [HttpGet("{authorId}")]
        public async Task<IActionResult> GetById(string authorId)
        {
            var author = await _authorService.GetAuthorByIdAsync(authorId);
            if (author == null)
                return NotFound();

            return Ok(author);
        }

        // POST /api/v1/authors → Add a new author
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AuthorRequest request)
        {
            var createdAuthor = await _authorService.AddAuthorAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { authorId = createdAuthor.AuthorId },
                createdAuthor
            );
        }

        // POST /api/v1/authors/bulk → Add multiple authors
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkCreate([FromBody] List<AuthorRequest> requests)
        {
            if (requests == null || !requests.Any())
            {
                return BadRequest("Request body cannot be empty.");
            }

            var invalidRequests = new List<string>();
            var createdAuthors = new List<AuthorResponse>();

            foreach (var request in requests)
            {
                try
                {
                    var createdAuthor = await _authorService.AddAuthorAsync(request);
                    createdAuthors.Add(createdAuthor);
                }
                catch (Exception ex)
                {
                    invalidRequests.Add(
                        $"Author with Name {request.AuthorName} failed: {ex.Message}"
                    );
                }
            }

            if (invalidRequests.Any())
            {
                return BadRequest(
                    new { Message = "Some authors could not be created.", Errors = invalidRequests }
                );
            }

            return Ok(createdAuthors);
        }

        // PATCH /api/v1/authors/{id} → Update a author
        [HttpPut("{authorId}")]
        public async Task<IActionResult> Update(string authorId, [FromBody] AuthorRequest request)
        {
            var updatedAuthor = await _authorService.UpdateAuthorAsync(authorId, request);
            if (updatedAuthor == null)
                return NotFound();

            return Ok(updatedAuthor);
        }

        // DELETE /api/v1/authors/{id} → Delete a author
        [HttpDelete("{authorId}")]
        public async Task<IActionResult> Delete(string authorId)
        {
            var deleted = await _authorService.DeleteAuthorAsync(authorId);
            if (!deleted)
                return NotFound();

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

            var books = await _authorService.GetAllBooksByAuthorIdAsync(page, limit, authorId);
            if (books == null)
                return NotFound();

            return Ok(books);
        }
    }
}
