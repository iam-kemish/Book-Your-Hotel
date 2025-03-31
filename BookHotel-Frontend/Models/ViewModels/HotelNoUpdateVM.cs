
using BookHotel_Frontend.Models.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookHotel_Frontend.Models.ViewModels
{
    public class HotelNoUpdateVM
    {
        public HotelNoUpdateVM()
        {
            hotelNoUpdateDTO = new HotelNoUpdateDTO();
        }
        public HotelNoUpdateDTO hotelNoUpdateDTO { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> HotelLists { get; set; }
    }
}
