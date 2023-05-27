using BookSalesApp.DTOs;

namespace BookSalesApp.IRepositories
{
    public interface IUserRepository
    {
        public Task<CustomReturnDTO> AdminLogin(LoginDTO loginDTO);
        public Task<LoginResponseDTO> Login(LoginDTO loginDTO);
        public Task<CustomReturnDTO> Register(RegisterDTO registerUserDTO);
    }
}