
using BookHotel_Frontend.Models.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookHotel_Frontend.Models.ViewModels
{
    public class HotelNoDeleteVM
    {
        public HotelNoDeleteVM()
        {
            hotelNoDTO = new HotelNoDTO();
        }
        public HotelNoDTO hotelNoDTO { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> HotelLists { get; set; }
    }
}
