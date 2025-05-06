using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Book_Your_Hotel.Models
{
    public class Hotels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } 
        public string Location { get; set; } 

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
       public string ImageUrl { get; set; }
        public int AvailableRooms { get; set; }
        public int Occupancy { get; set; }
        public int NumberOfRooms { get; set; }
       
        [Range(500, int.MaxValue, ErrorMessage = "Price must be greater than or equal to 500.")]
        public int Price { get; set; }
  
        public string ContactNumber { get; set; } = string.Empty;
    }
}
