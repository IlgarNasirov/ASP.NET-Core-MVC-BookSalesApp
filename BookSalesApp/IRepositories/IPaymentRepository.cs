using BookSalesApp.DTOs;

namespace BookSalesApp.IRepositories
{
    public interface IPaymentRepository
    {
        public Task<CustomReturnDTO> Pay(double amount, int userid);
        public IQueryable<PaymentResponseDTO> Payments(int userid);
        public HomeResponseDTO HomeResponse();
    }
}
