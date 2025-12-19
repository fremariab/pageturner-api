using System.Text.Json.Serialization; // Optional: If you need to rename fields for JSON

namespace PageTurner.Api.Models.Entities
{
    public class Book
    {
        public required string BookId { get; set; }

        public required string BookTitle { get; set; }

        public required string Author { get; set; }

        public required string ISBN { get; set; }

        public double Price { get; set; }

        public int StockQuantity { get; set; }

        public required string Genre { get; set; }

        public double AverageRating { get; set; }
    }
}
