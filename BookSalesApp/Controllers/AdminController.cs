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
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPaymentRepository _paymentRepository;

        private readonly IFileService _fileService;
        public AdminController(IGenreRepository genreRepository, IBookRepository bookRepository, IFileService fileService, IUserRepository userRepository, IPaymentRepository paymentRepository)
        {
            _genreRepository = genreRepository;
            _bookRepository = bookRepository;
            _fileService = fileService;
            _userRepository = userRepository;
            _paymentRepository = paymentRepository;
        }
        public IActionResult Home()
        {
            return View(_paymentRepository.HomeResponse());
        }
        public IActionResult AddGenre()
        {
            ViewData["title"] = "Add genre";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGenre(AddGenreDTO addGenreDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _genreRepository.AddGenre(addGenreDTO);
                if (result.Type == true)
                {
                    TempData["success"] = result.Message;
                    return RedirectToAction("AllGenres", "Admin");
                }
                ViewData["error"]=result.Message;
            }
            ViewData["title"] = "Add genre";
            return View();
        }
        public async Task<IActionResult> UpdateGenre(int id)
        {
            var result=await _genreRepository.GetGenre(id);
            if (result == null)
            {
                return RedirectToAction("AllGenres", "Admin");
            }
            ViewData["title"] = "Update genre";
            return View("AddGenre", result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateGenre(AddGenreDTO addGenreDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _genreRepository.UpdateGenre(addGenreDTO);
                if(result.Type == true)
                {
                    TempData["success"] = result.Message;
                    return RedirectToAction("AllGenres", "Admin");
                }
                if (result.Message == null)
                    return RedirectToAction("AllGenres", "Admin");
                ViewData["error"] = result.Message;
            }
            ViewData["title"] = "Update genre";
            return View("AddGenre");
        }
        public async Task<IActionResult>DeleteGenre(int id)
        {
            var result = await _genreRepository.DeleteGenre(id);
            if(result.Type==true)
                TempData["success"] = result.Message;
            return RedirectToAction("AllGenres", "Admin");
        }
        public IActionResult AllGenres()
        {
            return View(_genreRepository.AllGenres());
        }
        public IActionResult AddBook()
        {
            ViewData["title"] = "Add book";
            return View(_bookRepository.AllGenres());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBook(AddBookDTO addBookDTO)
        {
            if (ModelState.IsValid)
            {
                if (((addBookDTO.Image != null && _fileService.CheckImage(addBookDTO.Image) == true)
                    || (addBookDTO.Image == null)) && _fileService.CheckFile(addBookDTO.File) == true)
                {
                    var result=await _bookRepository.AddBook(addBookDTO);
                    if(result.Type==true)
                    TempData["success"] = result.Message;
                    return RedirectToAction("AllBooks", "Admin");
                }
                ViewData["error"] = "These file format(s) are not supported!";
            }
            ViewData["title"] = "Add book";
            return View(_bookRepository.AllGenres());
        }
        public async Task<IActionResult> UpdateBook(int id)
        {
            ViewData["title"] = "Update book";
            return View("AddBook", await _bookRepository.GetBook(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>UpdateBook(AddBookDTO addBookDTO)
        {
            if (ModelState.IsValid)
            {
                if (((addBookDTO.Image != null && _fileService.CheckImage(addBookDTO.Image) == true)
                    || (addBookDTO.Image == null)) && _fileService.CheckFile(addBookDTO.File) == true)
                {
                    var result = await _bookRepository.UpdateBook(addBookDTO);
                    if (result.Type == true)
                    {
                        TempData["success"] = result.Message;
                    }
                    return RedirectToAction("AllBooks", "Admin");
                }
                ViewData["error"] = "These file format(s) are not supported!";
            }
            ViewData["title"] = "Update book";
            return View("AddBook", _bookRepository.AllGenres());
        }
        public async Task<IActionResult>DeleteBook(int id)
        {
            var result = await _bookRepository.DeleteBook(id);
            if (result.Type == true)
                TempData["success"] = result.Message;
            return RedirectToAction("AllBooks", "Admin");
        }
        public IActionResult AllBooks()
        {
            return View(_bookRepository.AllBooks());
        }
        [AllowAnonymous]
        public IActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> AdminLogin(LoginDTO loginDTO)
        {
            if(ModelState.IsValid)
            {
                var result = await _userRepository.AdminLogin(loginDTO);
                if (result.Type == true)
                {
                    var claims = new List<Claim>
                    {
                    new Claim(ClaimTypes.Role, "Admin")
                    };
                    var userIdentity = new ClaimsIdentity(claims, "Login");
                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                    await HttpContext.SignInAsync(principal);
                    return RedirectToAction("Home", "Admin");
                }
                if (result.Type == false)
                    ViewData["error"] = result.Message;
            }
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("AdminLogin", "Admin");
        }
        [AllowAnonymous]
        public IActionResult PopularGenres()
        {
            return Ok(_genreRepository.PopularGenres());
        }
    }
}
