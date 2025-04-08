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
    public class HotelNumberController : Controller
    {
        private readonly IHotelNumberService _IHotelNo;
        private readonly IMapper _IMapper;
        private readonly IHotelService _IHotelService;

        public HotelNumberController(IHotelNumberService hotelNoService, IMapper mapper, IHotelService IHotelService)
        {
            _IHotelNo = hotelNoService;
            _IMapper = mapper;
            _IHotelService = IHotelService;
        }

        public async Task<IActionResult> Index()
        {
            List<HotelNoDTO> ResultedList = new();
            var response = await _IHotelNo.GetAllAsync<APIResponse>(HttpContext.Session.GetString("JWTToken"));
            if (response != null && response.IsSuccess)
            {
                ResultedList = JsonConvert.DeserializeObject<List<HotelNoDTO>>(Convert.ToString(response.Result));
            }
            return View(ResultedList);
        }

        public async Task<IActionResult> Create()
        {
            HotelNoCreateVM hotelNoCreateVM = new HotelNoCreateVM();
            var response = await _IHotelService.GetAllAsync<APIResponse>(HttpContext.Session.GetString("JWTToken"));
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
                var response = await _IHotelNo.CreateAsync<APIResponse>(hotelNoCreateVM.HotelNoCreateDTO, HttpContext.Session.GetString("JWTToken"));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Hotel number created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = response.Errors.FirstOrDefault() ?? "Error occurred while creating hotel number.";
                }
            }

            var res = await _IHotelService.GetAllAsync<APIResponse>(HttpContext.Session.GetString("JWTToken"));
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
            HotelNoUpdateVM hotelNoUpdateVM = new();
            var response = await _IHotelNo.GetAsync<APIResponse>(HotelNoId, HttpContext.Session.GetString("JWTToken"));
            if (response != null && response.IsSuccess)
            {
                HotelNoDTO hotelNoDto = JsonConvert.DeserializeObject<HotelNoDTO>(Convert.ToString(response.Result));
                hotelNoUpdateVM.hotelNoUpdateDTO = _IMapper.Map<HotelNoUpdateDTO>(hotelNoDto);
            }
            response = await _IHotelService.GetAllAsync<APIResponse>(HttpContext.Session.GetString("JWTToken"));
            if (response != null && response.IsSuccess)
            {
                hotelNoUpdateVM.HotelLists = JsonConvert.DeserializeObject<List<HotelsDTO>>(Convert.ToString(response.Result)).Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(hotelNoUpdateVM);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(HotelNoUpdateVM hotelNoUpdateVM)
        {
            if (ModelState.IsValid)
            {
                var response = await _IHotelNo.UpdateAsync<APIResponse>(hotelNoUpdateVM.hotelNoUpdateDTO, HttpContext.Session.GetString("JWTToken"));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Hotel number updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = response.Errors.FirstOrDefault() ?? "Error occurred while updating hotel number.";
                }
            }

            var res = await _IHotelService.GetAllAsync<APIResponse>(HttpContext.Session.GetString("JWTToken"));
            if (res != null && res.IsSuccess)
            {
                hotelNoUpdateVM.HotelLists = JsonConvert.DeserializeObject<List<HotelsDTO>>(Convert.ToString(res.Result)).Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            }
            return View(hotelNoUpdateVM);
        }

        public async Task<IActionResult> Delete(int HotelNoId)
        {
            HotelNoDeleteVM hotelNoDeleteVM = new();
            var response = await _IHotelNo.GetAsync<APIResponse>(HotelNoId, HttpContext.Session.GetString("JWTToken"));
            if (response != null && response.IsSuccess)
            {
                HotelNoDTO hotelNoDto = JsonConvert.DeserializeObject<HotelNoDTO>(Convert.ToString(response.Result));
                hotelNoDeleteVM.hotelNoDTO = hotelNoDto;
            }
            response = await _IHotelService.GetAllAsync<APIResponse>(HttpContext.Session.GetString("JWTToken"));
            if (response != null && response.IsSuccess)
            {
                hotelNoDeleteVM.HotelLists = JsonConvert.DeserializeObject<List<HotelsDTO>>(Convert.ToString(response.Result)).Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(hotelNoDeleteVM);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(HotelNoDeleteVM hotelNoDeleteVM)
        {
            var response = await _IHotelNo.DeleteAsync<APIResponse>(hotelNoDeleteVM.hotelNoDTO.HotelNumber, HttpContext.Session.GetString("JWTToken"));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Hotel number deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response.Errors.FirstOrDefault() ?? "Error occurred while deleting hotel number.";
            }
            return View(hotelNoDeleteVM);
        }
    }
}
