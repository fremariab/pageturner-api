using System.Text.Json.Serialization; // Optional: If you need to rename fields for JSON

namespace PageTurner.Api.Models.Entities
{
    public class Review
    {
        public required string ReviewId { get; set; }

        public required string ReviewerName { get; set; }

        public string? Comment { get; set; }

        public required int Rating { get; set; }

        public required string BookId { get; set; }
    }
}
