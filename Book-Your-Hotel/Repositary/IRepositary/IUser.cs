using Book_Your_Hotel.Models;
using Book_Your_Hotel.Models.DTOs;

namespace Book_Your_Hotel.Repositary.IRepositary
{
    public interface IUser
    {
        bool IsUniqueUser(string user);
        Task<LoginResponseDTO> Login(LoginRequestDTO request);
        Task<UserDTO> Register(RegisterationRequestDTO registerationRequestDTO);
    }
}
