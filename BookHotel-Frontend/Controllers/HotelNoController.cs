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
                        ModelState.AddModelError("Error", response.Errors.FirstOrDefault());
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
            HotelNoUpdateVM hotelNoUpdateVM = new();
            var response = await _IHotelNo.GetAsync<APIResponse>(HotelNoId);
            if (response != null && response.IsSuccess)
            {
               HotelNoDTO hotelNoDto = JsonConvert.DeserializeObject<HotelNoDTO>(Convert.ToString(response.Result));
             hotelNoUpdateVM.hotelNoUpdateDTO =   _IMapper.Map<HotelNoUpdateDTO>(hotelNoDto);
            }
            var res = await _IHotelService.GetAllAsync<APIResponse>();
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(HotelNoUpdateVM hotelNoUpdateVM)
        {
            if (ModelState.IsValid)
            {

                var response = await _IHotelNo.UpdateAsync<APIResponse>(hotelNoUpdateVM.hotelNoUpdateDTO);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    if (response.Errors.Count > 0)
                    {
                        ModelState.AddModelError("Error", response.Errors.FirstOrDefault());
                    }
                }
            }
            var res = await _IHotelService.GetAllAsync<APIResponse>();
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
            var response = await _IHotelNo.GetAsync<APIResponse>(HotelNoId);
            if (response != null && response.IsSuccess)
            {
                HotelNoDTO HotelNoDto = JsonConvert.DeserializeObject<HotelNoDTO>(Convert.ToString(response.Result));
                hotelNoDeleteVM.hotelNoDTO = _IMapper.Map<HotelNoDTO>(hotelNoDeleteVM.hotelNoDTO);
            }
            var res = await _IHotelService.GetAllAsync<APIResponse>();
            if (res != null && res.IsSuccess)
            {
               hotelNoDeleteVM.HotelLists = JsonConvert.DeserializeObject<List<HotelsDTO>>(Convert.ToString(response.Result)).Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });

            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(HotelNoDeleteVM hotelNoDeleteVM)
        {
            if (ModelState.IsValid)
            {
                var response = await _IHotelNo.DeleteAsync<APIResponse>(hotelNoDeleteVM.hotelNoDTO.HotelNumber);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));

                }
            }
            return View(hotelNoDeleteVM);
        }
    }
}