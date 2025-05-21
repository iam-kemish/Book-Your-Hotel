using BookHotel_Frontend.Models.DTOs;

namespace BookHotel_Frontend.Services.IServices
{
    public interface IToken
    {
        void SetToken(LoginResponseDTO loginResponseDTO);
        LoginResponseDTO? GetToken();
        void ClearToken();
    }
}
