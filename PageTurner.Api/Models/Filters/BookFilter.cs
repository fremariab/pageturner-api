namespace PageTurner.Api.Models.Filters
{
    public class BookFilter
    {
        public string? BookTitle { get; set; }
        public string? Author { get; set; }
        public string? ISBN { get; set; }
        public double? Price { get; set; }
        public int? StockQuantity { get; set; }
        public string? Genre { get; set; }
        public double? AverageRating { get; set; }
        public bool? InStock { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }
    }
}
