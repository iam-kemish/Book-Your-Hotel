using System.ComponentModel.DataAnnotations;

namespace Book_Your_Hotel.Models.DTOs
{
    public class HotelsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string ImageUrl { get; set; }
        public string? ImageLocalPath { get; set; }
      
        public int Occupancy { get; set; }
        public int NumberOfRooms { get; set; }

        public int AvailableRooms { get; set; }
        public int Price { get; set; }
        public string ContactNumber { get; set; }
    }
}
