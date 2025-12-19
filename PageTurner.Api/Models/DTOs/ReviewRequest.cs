namespace PageTurner.Api.Models.DTOs
{
    public class ReviewRequest
    {
        public required string ReviewerName { get; set; } = string.Empty;

        public string? Comment { get; set; } = string.Empty;

        public required int Rating { get; set; }

        public required string BookId { get; set; }
    }
}
