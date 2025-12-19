using System.Text.Json.Serialization; // Optional: If you need to rename fields for JSON

namespace PageTurner.Api.Models.Entities
{
    public class Author
    {
        public required string AuthorId { get; set; }

        public required string AuthorName { get; set; }
        public string? AuthorBio { get; set; }
    }
}
