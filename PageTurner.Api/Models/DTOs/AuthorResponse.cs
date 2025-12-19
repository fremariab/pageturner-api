namespace PageTurner.Api.Models.DTOs
{
    public class AuthorResponse
    {
        public string AuthorId { get; set; } = string.Empty;
        public string? AuthorName { get; set; } = string.Empty;
        public string? AuthorBio { get; set; } = string.Empty;
    }
}
