namespace BookSalesApp.DTOs
{
    public class LoginResponseDTO
    {
        public bool Type { get; set; }
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Message { get; set; }
    }
}
