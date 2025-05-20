using System.ComponentModel.DataAnnotations;

namespace Book_Your_Hotel.Models.DTOs
{
    public class HotelCreateDTO
    {
        [MaxLength(40)]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }


        public string? ImageUrl { get; set; }

        public IFormFile? Image { get; set; }


        public int NumberOfRooms { get; set; }


        public int Occupancy { get; set; }

        [Range(500, int.MaxValue, ErrorMessage = "Price must be greater than or equal to 500.")]
        public int Price { get; set; }

        public string ContactNumber { get; set; }

    }
}
