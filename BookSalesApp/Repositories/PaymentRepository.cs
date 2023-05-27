using BookSalesApp.DTOs;
using BookSalesApp.IRepositories;
using BookSalesApp.Models;

namespace BookSalesApp.Repositories
{
    public class PaymentRepository:IPaymentRepository
    {
        private readonly BookSalesDbContext _db;
        public PaymentRepository(BookSalesDbContext bookSalesDbContext)
        {
            _db = bookSalesDbContext;
        }
        public async Task<CustomReturnDTO> Pay(double amount, int userid)
        {
            var result = _db.Carts.Where(c => c.Userid == userid && c.Status == true && c.Issold == false);
            if (result == null)
            {
                return new CustomReturnDTO { Type = false };
            }
            foreach (var item in result)
            {
                item.Status = false;
                item.Issold = true;
            }
            var payment = new Payment { Paymentdate = DateTime.Now, Paymentid = Guid.NewGuid().ToString(), Totalamount = amount, Userid = userid };
            await _db.Payments.AddAsync(payment);
            await _db.SaveChangesAsync();
            return new CustomReturnDTO { Type = true, Message = "The operation finished successfully!" };
        }
        public IQueryable<PaymentResponseDTO> Payments(int userid)
        {
            return from p in _db.Payments
                   where p.Userid == userid
                   select new PaymentResponseDTO
                   {
                       Paymentdate = p.Paymentdate,
                       Paymentid = p.Paymentid,
                       Totalamount = p.Totalamount
                   };
        }
        public HomeResponseDTO HomeResponse()
        {
            var homeResponse=new HomeResponseDTO();
            homeResponse.TotalSales = _db.Payments.Sum(p => p.Totalamount);
            homeResponse.TodayTotalSales = _db.Payments.Where(p => p.Paymentdate.Date == DateTime.Now.Date).Sum(p=>p.Totalamount);
            homeResponse.Users = _db.Users.Where(u => u.Isadmin == false).Count();
            return homeResponse;
        }
    }
}
