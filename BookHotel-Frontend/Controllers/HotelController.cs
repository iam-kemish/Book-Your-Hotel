using AutoMapper;
using BookHotel_Frontend.Models.DTOs;
using BookHotel_Frontend.Services.IServices;
using BookHotel_Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using APIResponse = BookHotel_Frontend.Models.APIResponse;

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

        [Authorize]
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HotelCreateDTO hotelCreateDTO)
        {
            if (ModelState.IsValid)
            {
                var response = await _IHotel.CreateAsync<APIResponse>(hotelCreateDTO );
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Hotel created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Errors", response?.Errors?.FirstOrDefault() ?? "Error occurred while creating the hotel. Check if user is authorized.");
                }
            }
            return View(hotelCreateDTO);
        }

        public async Task<IActionResult> Update(int hotelId)
        {
            var response = await _IHotel.GetAsync<APIResponse>(hotelId );
            if (response != null && response.IsSuccess)
            {
                HotelsDTO hotelsDTO = JsonConvert.DeserializeObject<HotelsDTO>(Convert.ToString(response.Result));
                return View(_IMapper.Map<HotelUpdateDTO>(hotelsDTO));
            }
            return BadRequest();
        }

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
                    ModelState.AddModelError("Errors", response?.Errors?.FirstOrDefault() ?? "Error occurred while updating hotel. Check if user is authorized.");
                }
            }
            return View(hotelUpdateDTO);
        }

        public async Task<IActionResult> Delete(int HotelId)
        {
            var response = await _IHotel.GetAsync<APIResponse>(HotelId );
            if (response != null && response.IsSuccess)
            {
                HotelsDTO hotelsDTO = JsonConvert.DeserializeObject<HotelsDTO>(Convert.ToString(response.Result));
                return View(hotelsDTO);
            }
            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(HotelsDTO hotelsDTO)
        {
            var response = await _IHotel.DeleteAsync<APIResponse>(hotelsDTO.Id );
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Hotel deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("Errors", response?.Errors?.FirstOrDefault() ?? "Error occurred while deleting hotel. Check if user is authorized.");
            }
            return View(hotelsDTO);
        }
    }
}
