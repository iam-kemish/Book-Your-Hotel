using AutoMapper;

using BookHotel_Frontend.Models;
using BookHotel_Frontend.Models.DTOs;
using BookHotel_Frontend.Models.ViewModels;
using BookHotel_Frontend.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace BookHotel_Frontend.Controllers
{
    public class HotelNoController : Controller
    {
        private readonly IHotelNoService _IHotelNo;
        private readonly IMapper _IMapper;
        private readonly IHotelService _IHotelService;
        public HotelNoController(IHotelNoService hotelNoService, IMapper mapper, IHotelService IHotelService)
        {
            _IHotelNo = hotelNoService;
            _IMapper = mapper;
           _IHotelService = IHotelService;
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
            HotelNoCreateVM hotelNoCreateVM = new HotelNoCreateVM();
            var response = await _IHotelService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                hotelNoCreateVM.HotelLists = JsonConvert.DeserializeObject<List<HotelsDTO>>(Convert.ToString(response.Result)).Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });

            }
                return View(hotelNoCreateVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HotelNoCreateVM hotelNoCreateVM)
        {

            if (ModelState.IsValid)
            {

                var response = await _IHotelNo.CreateAsync<APIResponse>(hotelNoCreateVM.HotelNoCreateDTO);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));

                } else
                {
                    if (response.Errors.Count > 0)
                    {
                        ModelState.AddModelError("Error", "Invalid id, seems you have tried to manipulate already exisiting data. Please recheck");
                    }
            }
            }

            
            var res = await _IHotelService.GetAllAsync<APIResponse>();
            if (res != null && res.IsSuccess)
            {
                hotelNoCreateVM.HotelLists = JsonConvert.DeserializeObject<List<HotelsDTO>>(Convert.ToString(res.Result)).Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });

            }
            return View(hotelNoCreateVM);

          
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