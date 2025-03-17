using System.ComponentModel.DataAnnotations;

namespace BookHotel_Frontend.Models.DTOs
{
    public class HotelNoUpdateDTO
    {
        [Required]
        public int HotelNumber { get; set; }
        [Required]
        public int HotelID { get; set; }
        public string SpecialDetails { get; set; }

    }
}
