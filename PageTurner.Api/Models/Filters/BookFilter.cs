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
        public bool? InStock { get; set; } // Filter for books that are in stock
        public double? MinPrice { get; set; } // Minimum price for price range filtering
        public double? MaxPrice { get; set; } // Maximum price for price range filtering
    }
}
