using BookSalesApp.DTOs;
using BookSalesApp.IRepositories;
using BookSalesApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BookSalesApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BookSalesDbContext _db;
        public UserRepository(BookSalesDbContext bookSalesDbContext)
        {
            _db = bookSalesDbContext;
        }
        public async Task<CustomReturnDTO> AdminLogin(LoginDTO loginDTO)
        {
            var result = await _db.Users.Where(u => u.Username == loginDTO.Username && u.Password == loginDTO.Password && u.Isadmin == true).FirstOrDefaultAsync();
            if (result == null)
                return new CustomReturnDTO { Type = false, Message = "Invalid username or password!" };
            return new CustomReturnDTO { Type = true };
        }
        public async Task<LoginResponseDTO> Login(LoginDTO loginDTO)
        {
            var user = await _db.Users.Where(u => u.Username == loginDTO.Username && u.Isadmin == false).FirstOrDefaultAsync();
            var errorMessage = "Invalid username or password!";
            if (user == null)
            {
                return new LoginResponseDTO { Type = false, Message = errorMessage };
            }
            if (!BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password))
            {
                return new LoginResponseDTO { Type = false, Message = errorMessage };
            }
            return new LoginResponseDTO { Type = true, Id = user.Id, Username = user.Username };
        }
        public async Task<CustomReturnDTO> Register(RegisterDTO registerUserDTO)
        {
            var user = await _db.Users.Where(u => u.Username == registerUserDTO.Username && u.Isadmin == false).FirstOrDefaultAsync();
            if (user == null)
            {
                var newUser = new User();
                newUser.Password = BCrypt.Net.BCrypt.HashPassword(registerUserDTO.Password);
                newUser.Username = registerUserDTO.Username;
                await _db.Users.AddAsync(newUser);
                await _db.SaveChangesAsync();
                return new CustomReturnDTO { Type = true, Message = "User successfully registered!" };
            }
            return new CustomReturnDTO { Type = false, Message = "This username already exists!" };
        }
    }
}
