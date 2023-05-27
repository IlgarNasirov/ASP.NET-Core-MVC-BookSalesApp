using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookSalesApp.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public IActionResult Error404(int code)
        {
            return View();
        }
        public IActionResult Error500()
        {
            return View();
        }
    }
}