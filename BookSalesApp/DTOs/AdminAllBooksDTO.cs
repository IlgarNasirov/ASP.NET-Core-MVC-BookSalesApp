using System.ComponentModel.DataAnnotations;

namespace BookSalesApp.DTOs
{
    public class AdminAllBooksDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Author { get; set; } = null!;
        public double? Price { get; set; }
    }
}