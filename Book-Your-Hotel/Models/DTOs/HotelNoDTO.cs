using System.ComponentModel.DataAnnotations;

namespace Book_Your_Hotel.Models.DTOs
{
    public class HotelNoDTO
    {
        [Required]
        public int HotelNumber { get; set; }
        public string SpecialDetails { get; set; }

    }
}
