using BookSalesApp.Models;
using System.ComponentModel.DataAnnotations;

namespace BookSalesApp.DTOs
{
    public class AddBookDTO
    {
        public int Id { get; set; }
        public IFormFile? Image { get; set; }
        [Required]
        public IFormFile File { get; set; } = null!;
        public IQueryable<string>? Genres { get; set; }
        [Required]
        public string Genre { get; set; } = null!;
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        [MinLength(3, ErrorMessage = "The Author field must be at least 3 characters long.")]
        public string Author { get; set; } = null!;
        [Required]
        [MinLength(3, ErrorMessage = "The Description field must be at least 3 characters long.")]
        public string Description { get; set; } = null!;
        [Required]
        [Range(0, double.MaxValue)]
        public double? Price { get; set; }
    }
}
