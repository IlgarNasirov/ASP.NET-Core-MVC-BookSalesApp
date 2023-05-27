using BookSalesApp.DTOs;
using BookSalesApp.IRepositories;
using BookSalesApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BookSalesApp.Repositories
{
    public class CartRepository:ICartRepository
    {
        private readonly BookSalesDbContext _db;
        public CartRepository(BookSalesDbContext bookSalesDbContext)
        {
            _db = bookSalesDbContext;
        }
        public async Task<CustomReturnDTO> AddToCart(int id, int? userid)
        {
            var result = await _db.Books.FindAsync(id);
            if (result == null)
            {
                return new CustomReturnDTO { Type = false };
            }
            var check = await _db.Carts.Where(c => c.Userid == userid && c.Bookid == id && (c.Status == true || c.Issold == true)).FirstOrDefaultAsync();
            if (check != null)
            {
                return new CustomReturnDTO { Type = false };
            }
            var r = await _db.Carts.Where(c => c.Userid == userid && c.Bookid == id && c.Status == false && c.Issold == false).FirstOrDefaultAsync();
            if (r != null)
                r.Status = true;
            else
            {
                var cart = new Cart { Bookid = id, Userid = Convert.ToInt32(userid) };
                _db.Carts.Add(cart);
            }
            await _db.SaveChangesAsync();
            return new CustomReturnDTO { Type = true, Message = "The book added to cart successfully!" };
        }
        public IQueryable<AdminAllBooksDTO> Cart(int userid)
        {
            return from c in _db.Carts
                   join b in _db.Books on c.Bookid equals b.Id
                   where c.Userid == userid && c.Status == true && c.Issold == false
                   select new AdminAllBooksDTO
                   {
                       Id = c.Id,
                       Author = b.Author,
                       Name = b.Name,
                       Price = b.Price
                   };
        }
        public async Task<CustomReturnDTO> DeleteFromCart(int id, int userid)
        {
            var result = await _db.Carts.Where(c => c.Userid == userid && c.Id == id && c.Status == true && c.Issold == false).FirstOrDefaultAsync();
            if (result == null)
            {
                return new CustomReturnDTO { Type = false };
            }
            result.Status = false;
            await _db.SaveChangesAsync();
            return new CustomReturnDTO { Type = true, Message = "The book deleted from cart successfully!" };
        }
    }
}
