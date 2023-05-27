using BookSalesApp.Models;

namespace BookSalesApp.DTOs
{
    public class IndexResponseDTO
    {
        public int Id { get; set; }
        public string? Imageurl { get; set; }
        public string Genre { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Price { get; set; }
    }
}