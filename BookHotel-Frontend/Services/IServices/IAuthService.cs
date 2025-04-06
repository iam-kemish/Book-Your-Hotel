using BookHotel_Frontend.Models.DTOs;

namespace BookHotel_Frontend.Services.IServices
{
    public interface IAuthService
    {
        Task<T> LoginAsync<T> (LoginRequestDTO loginRequestDTO);
        Task<T> RegisterAsync<T>(RegisterRequestDTO registerRequestDTO);
    }
}
