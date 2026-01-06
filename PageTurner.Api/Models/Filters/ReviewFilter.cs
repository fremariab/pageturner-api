namespace PageTurner.Api.Models.Filters
{
    public class ReviewFilter
    {
        public string? ReviewerName { get; set; }
        public string? Comment { get; set; }
        public int? Rating { get; set; }
        public string? BookTitle { get; set; }
    }
}
