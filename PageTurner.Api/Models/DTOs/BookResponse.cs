namespace PageTurner.Api.Models.DTOs
{
    public class BookResponse
    {
        public string BookId { get; set; } = string.Empty;
        public string BookTitle { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public double Price { get; set; }
        public int StockQuantity { get; set; }
        public string Genre { get; set; } = string.Empty;
    }
}
