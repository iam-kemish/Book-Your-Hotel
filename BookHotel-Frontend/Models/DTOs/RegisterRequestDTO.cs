﻿namespace BookHotel_Frontend.Models.DTOs
{
    public class RegisterRequestDTO
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string? Role { get; set; }
    }
}
