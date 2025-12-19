namespace PageTurner.Api.Models.DTOs
{
    public class PagedResponse<T>
    {
        public bool Success { get; set; } = true;
        public List<T> Data { get; set; } = new List<T>();
        public Pagination Pagination { get; set; } = new Pagination();
    }

    public class Pagination
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int Limit { get; set; }
    }
}
