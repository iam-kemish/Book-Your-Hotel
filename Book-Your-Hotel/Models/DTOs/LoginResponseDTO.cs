namespace Book_Your_Hotel.Models.DTOs
{
    public class LoginResponseDTO
    {
        public LocalUser User { get; set; }
        public string Token { get; set; }
    }
}
