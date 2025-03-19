using System.ComponentModel.DataAnnotations;

namespace Book_Your_Hotel.Models.DTOs
{
    public class HotelNoDTO
    {
        [Required]
        public int HotelNumber { get; set; }
        [Required]
        public int HotelID { get; set; }
        public string SpecialDetails { get; set; }

        public HotelsDTO Hotels { get; set; }

    }
}
