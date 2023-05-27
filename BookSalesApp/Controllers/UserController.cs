using BookSalesApp.DTOs;
using BookSalesApp.IRepositories;
using BookSalesApp.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookSalesApp.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IGetUserService _getUserService;
        private readonly ICartRepository _cartRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IFileService _fileService;

        public UserController(IUserRepository userRepository, IGetUserService getUserService, ICartRepository cartRepository, IPaymentRepository paymentRepository, IBookRepository bookRepository, IFileService fileService)
        {
            _userRepository = userRepository;
            _getUserService = getUserService;
            _cartRepository = cartRepository;
            _paymentRepository = paymentRepository;
            _bookRepository = bookRepository;
            _fileService = fileService;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var v = _getUserService.GetUserId();
            if (v > 0)
            {
                ViewData["allow"] = "Yes";
            }
            return View(await _bookRepository.AllBooks(v));
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (ModelState.IsValid)
            {
               var result= await _userRepository.Login(loginDTO);
                if(result.Type==true)
                {
                    var claims = new List<Claim>
                    {
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim(ClaimTypes.NameIdentifier, result.Id.ToString()),
                    new Claim(ClaimTypes.Name, result.Username)
                    };
                    var userIdentity = new ClaimsIdentity(claims, "Login");
                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                    await HttpContext.SignInAsync(principal);
                    return RedirectToAction("Index", "User");
                }
                ViewData["error"] = result.Message;
            }
            return View();
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (ModelState.IsValid)
            {
                var result= await _userRepository.Register(registerDTO);
                if (result.Type == true)
                {
                    TempData["success"] = result.Message;
                    return RedirectToAction("Login", "User");
                }
                ViewData["error"]=result.Message;
            }
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var v = _getUserService.GetUserId();
            var result = await _bookRepository.GetBook(id, v);
            if (result == null)
            {
                return RedirectToAction("Index", "User");
            }
            if(v>0)
                ViewData["allow"] = "Yes";
            return View(result);
        }
        public async Task<IActionResult> AddToCart(int id)
        {
            var result= await _cartRepository.AddToCart(id, _getUserService.GetUserId());
            if (result.Type == false)
            {
                return RedirectToAction("Index", "User");
            }
            TempData["success"] = result.Message;
            return RedirectToAction("Cart", "User");
        }
        public IActionResult Cart()
        {
            return View(_cartRepository.Cart(_getUserService.GetUserId()));
        }
        public async Task<IActionResult> DeleteFromCart(int id)
        {
            var result = await _cartRepository.DeleteFromCart(id, _getUserService.GetUserId());
            if (result.Type == false)
            {
                return RedirectToAction("Index", "User");
            }
            TempData["success"] = result.Message;
            return RedirectToAction("Cart", "User");
        }
        public IActionResult Payments()
        {
            return View(_paymentRepository.Payments(_getUserService.GetUserId()));
        }
        public async Task<IActionResult> Pay(double amount)
        {
            var result = await _paymentRepository.Pay(amount, _getUserService.GetUserId());
            if (result.Type == false)
            {
                return RedirectToAction("Index","User");
            }
            TempData["success"] = result.Message;
            return RedirectToAction("Payments","User");
        }
        public IActionResult YourBooks()
        {
            return View(_bookRepository.YourBooks(_getUserService.GetUserId()));
        }
        public async Task<IActionResult> Download(int id)
        {
            var result = await _bookRepository.Check(id, _getUserService.GetUserId());
            if (result != null)
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "files", result);
                if(_fileService.CheckFileExistance(filePath)==true)
                {
                    return PhysicalFile(filePath, "application/pdf", result);
                }
            }
            return RedirectToAction("YourBooks","User");
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult>SearchBook(string name)
        {
            var v = _getUserService.GetUserId();
            if (v > 0)
            {
                ViewData["allow"] = "Yes";
            }
            if (string.IsNullOrEmpty(name))
              return View("Index", await _bookRepository.AllBooks(v));
            return View("Index", await _bookRepository.SearchBooks(v, name));
        }
    }
}