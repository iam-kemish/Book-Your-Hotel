using System.ComponentModel.DataAnnotations;

namespace BookHotel_Frontend.Models.DTOs
{
    public class HotelsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string ImageUrl { get; set; }
        public int NumberOfRooms { get; set; }
        public int Price { get; set; }
        public string ContactNumber { get; set; }
    }
}
