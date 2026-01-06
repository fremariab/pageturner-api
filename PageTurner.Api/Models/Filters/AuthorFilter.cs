namespace PageTurner.Api.Models.Filters
{
    public class AuthorFilter
    {
        public string? AuthorName { get; set; }
        public string? AuthorBio { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }
    }
}
