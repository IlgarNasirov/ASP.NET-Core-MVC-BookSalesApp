using System.ComponentModel.DataAnnotations;

namespace BookSalesApp.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [MinLength(5, ErrorMessage = "The username field must be at least 5 characters long.")]
        public string Username { get; set; } = null!;
        [Required]
        [MinLength(6, ErrorMessage = "The password field must be at least 6 characters long.")]
        public string Password { get; set; } = null!;
        [Compare("Password", ErrorMessage = "The repassword field must be equal to the password field.")]
        public string RePassword { get; set; } = null!;
    }
}
