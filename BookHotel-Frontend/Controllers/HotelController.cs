using AutoMapper;
using BookHotel_Frontend.Models;
using BookHotel_Frontend.Models.DTOs;
using BookHotel_Frontend.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookHotel_Frontend.Controllers
{
    public class HotelController : Controller
    {
        private readonly IHotelService _IHotel;
        private readonly IMapper _IMapper;

        public HotelController(IHotelService hotelService, IMapper mapper)
        {
            _IHotel = hotelService;
            _IMapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            List<HotelsDTO> ResultedList = new();
            var response = await _IHotel.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                ResultedList = JsonConvert.DeserializeObject<List<HotelsDTO>>(Convert.ToString(response.Result));
            }
            return View(ResultedList);
        }
        
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [Authorize(Roles ="Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HotelCreateDTO hotelCreateDTO)
        {
            if (ModelState.IsValid)
            {
                var response = await _IHotel.CreateAsync<APIResponse>(hotelCreateDTO);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Hotel created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = response.Errors.FirstOrDefault() ?? "Error occurred while creating hotel.";
                }
            }
            return View(hotelCreateDTO);
        }

        public async Task<IActionResult> Update(int hotelId)
        {
            var response = await _IHotel.GetAsync<APIResponse>(hotelId);
            if (response != null && response.IsSuccess)
            {
                HotelsDTO hotelsDTO = JsonConvert.DeserializeObject<HotelsDTO>(Convert.ToString(response.Result));
                return View(_IMapper.Map<HotelUpdateDTO>(hotelsDTO));
            }
            return NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(HotelUpdateDTO hotelUpdateDTO)
        {
            if (ModelState.IsValid)
            {
                var response = await _IHotel.UpdateAsync<APIResponse>(hotelUpdateDTO);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Hotel updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = response.Errors.FirstOrDefault() ?? "Error occurred while updating hotel.";
                }
            }
            return View(hotelUpdateDTO);
        }

        public async Task<IActionResult> Delete(int HotelId)
        {
            var response = await _IHotel.GetAsync<APIResponse>(HotelId);
            if (response != null && response.IsSuccess)
            {
                HotelsDTO hotelsDTO = JsonConvert.DeserializeObject<HotelsDTO>(Convert.ToString(response.Result));
                return View(hotelsDTO);
            }
            return NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(HotelsDTO hotelsDTO)
        {
            var response = await _IHotel.DeleteAsync<APIResponse>(hotelsDTO.Id);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Hotel deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response.Errors.FirstOrDefault() ?? "Error occurred while deleting hotel.";
            }
            return View(hotelsDTO);
        }
    }
}
