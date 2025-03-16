using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Book_Your_Hotel.Models
{
    public class HotelNumbers
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int HotelNumber { get; set; }

        [ForeignKey("Hotels")]
        public int HotelID { get; set; } 
        public Hotels Hotels { get; set; }
        public string SpecialDetails { get;set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}
