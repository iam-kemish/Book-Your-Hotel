using AutoMapper;
using BookHotel_Frontend.Models;
using BookHotel_Frontend.Models.DTOs;
using BookHotel_Frontend.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookHotel_Frontend.Controllers
{
    public class HotelNoController : Controller
    {
        private readonly IHotelNoService _IHotelNo;
        private readonly IMapper _IMapper;

        public HotelNoController(IHotelNoService hotelNoService, IMapper mapper)
        {
            _IHotelNo = hotelNoService;
            _IMapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            List<HotelNoDTO> ResultedList = new();
            var response = await _IHotelNo.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                ResultedList = JsonConvert.DeserializeObject<List<HotelNoDTO>>(Convert.ToString(response.Result));

            }
            return View(ResultedList);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HotelNoCreateDTO hotelNoCreateDTO)
        {
            if (ModelState.IsValid)
            {

                var response = await _IHotelNo.CreateAsync<APIResponse>(hotelNoCreateDTO);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));

                }
            }
            return View(hotelNoCreateDTO);
        }

        public async Task<IActionResult> Update(int HotelNoId)
        {
            var response = await _IHotelNo.GetAsync<APIResponse>(HotelNoId);
            if (response != null && response.IsSuccess)
            {
                HotelNoDTO hotelNoDTO = JsonConvert.DeserializeObject<HotelNoDTO>(Convert.ToString(response.Result));
                return View(_IMapper.Map<HotelNoUpdateDTO>(hotelNoDTO));
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(HotelNoUpdateDTO hotelNoUpdateDTO)
        {
            if (ModelState.IsValid)
            {
                var response = await _IHotelNo.UpdateAsync<APIResponse>(hotelNoUpdateDTO);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            return View(hotelNoUpdateDTO);
        }
    
     public async Task<IActionResult> Delete(int HotelNoId)
        {
            var response = await _IHotelNo.GetAsync<APIResponse>(HotelNoId);
            if (response != null && response.IsSuccess)
            {
                HotelNoDTO hotelsNoDTO = JsonConvert.DeserializeObject<HotelNoDTO>(Convert.ToString(response.Result));
                return View(hotelsNoDTO);
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(HotelNoDTO hotelsNoDTO)
        {
            if (ModelState.IsValid)
            {
                var response = await _IHotelNo.DeleteAsync<APIResponse>(hotelsNoDTO.HotelNumber);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));

                }
            }
            return View(hotelsNoDTO);
        }
    }
}