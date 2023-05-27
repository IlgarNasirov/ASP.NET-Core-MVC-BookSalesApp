namespace BookSalesApp.DTOs
{
    public class PaymentResponseDTO
    {
        public double Totalamount { get; set; }
        public DateTime Paymentdate { get; set; }
        public string Paymentid { get; set; } = null!;

    }
}