using Book_Your_Hotel.Models.DTOs;

namespace Book_Your_Hotel.Models
{
    public class HotelStore
    {
        public static List<HotelsDTO> HotelsStores = new List<HotelsDTO>
            {
                new HotelsDTO { Id = 1, Name = "Everest"},
                new HotelsDTO{Id=2, Name="Mypit"}
            };
    }
}
