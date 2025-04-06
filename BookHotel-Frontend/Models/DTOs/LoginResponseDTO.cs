using Book_Your_Hotel.Models;

namespace BookHotel_Frontend.Models.DTOs
{
    public class LoginResponseDTO
    {
        public UserDTO User { get; set; }
        public string Token { get; set; }
    }
}
