using BookSalesApp.DTOs;

namespace BookSalesApp.IRepositories
{
    public interface ICartRepository
    {
        public Task<CustomReturnDTO> AddToCart(int id, int? userid);
        public IQueryable<AdminAllBooksDTO> Cart(int userid);
        public Task<CustomReturnDTO> DeleteFromCart(int id, int userid);
    }
}
