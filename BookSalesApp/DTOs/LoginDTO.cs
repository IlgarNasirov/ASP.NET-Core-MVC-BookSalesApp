using System.ComponentModel.DataAnnotations;

namespace BookSalesApp.DTOs
{
    public class LoginDTO
    {
        [Required]
        [MinLength(3, ErrorMessage = "The Username field must be at least 3 characters long.")]
        public string Username { get; set; } = null!;
        [Required]
        [MinLength(6, ErrorMessage = "The Password field must be at least 6 characters long.")]
        public string Password { get; set; }= null!;
    }
}
