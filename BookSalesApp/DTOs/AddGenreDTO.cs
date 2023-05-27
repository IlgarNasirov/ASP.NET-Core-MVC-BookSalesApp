using System.ComponentModel.DataAnnotations;

namespace BookSalesApp.DTOs
{
    public class AddGenreDTO
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "The Name field must be at least 3 characters long.")]
        public string Name { get; set; } = null!;
    }
}