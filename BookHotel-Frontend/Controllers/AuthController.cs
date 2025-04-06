using BookHotel_Frontend.Models;
using BookHotel_Frontend.Models.DTOs;
using BookHotel_Frontend.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BookHotel_Frontend.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _IAuth;

        public AuthController(IAuthService authService)
        {
            _IAuth = authService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register() { 

         return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterRequestDTO registerRequestDTO)
        {
            APIResponse aPIResponse = await _IAuth.RegisterAsync<APIResponse>(registerRequestDTO);
            if(aPIResponse == null && aPIResponse.IsSuccess)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

    }
}
