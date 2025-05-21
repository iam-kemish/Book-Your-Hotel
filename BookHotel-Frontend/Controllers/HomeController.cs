using System.Diagnostics;
using AutoMapper;
using BookHotel_Frontend.Models;
using BookHotel_Frontend.Models.DTOs;
using BookHotel_Frontend.Services.IServices;
using BookHotel_Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookHotel_Frontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHotelService _IHotel;
        private readonly IMapper _IMapper;

        public HomeController(IHotelService hotelService, IMapper mapper)
        {
            _IHotel = hotelService;
            _IMapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            List<HotelsDTO> ResultedList = new();
            // this isnt now needed because we are now getting token using cookies 
            var response = await _IHotel.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                ResultedList = JsonConvert.DeserializeObject<List<HotelsDTO>>(Convert.ToString(response.Result));

            }
            return View(ResultedList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

      
      
    }
}
