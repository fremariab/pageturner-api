namespace PageTurner.Api.Models.DTOs
{
    public class ReviewResponse
    {
        public required string ReviewId { get; set; }
        public required string ReviewerName { get; set; } = string.Empty;

        public string? Comment { get; set; } = string.Empty;

        public required int Rating { get; set; }
        public string? BookId { get; set; } = string.Empty;

        public string? BookTitle { get; set; } = string.Empty;
        public string? Author { get; set; } = string.Empty;
    }
}
