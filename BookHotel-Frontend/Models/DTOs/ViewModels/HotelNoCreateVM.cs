using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookHotel_Frontend.Models.DTOs.ViewModels
{
    public class HotelNoCreateVM
    {
        public HotelNoCreateDTO HotelNoCreateDTO { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> HotelLists { get; set; }
    }
}
