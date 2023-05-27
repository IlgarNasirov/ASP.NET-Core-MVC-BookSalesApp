using System.Security.Claims;
using BookSalesApp.IServices;

namespace BookSalesApp.Services
{
    public class GetUserService:IGetUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public int GetUserId()
        {
            return Convert.ToInt32(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}