using System.Security.Claims;
using BookHotel_Frontend.Models;
using BookHotel_Frontend.Models.DTOs;
using BookHotel_Frontend.Services.IServices;
using BookHotel_Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
            LoginRequestDTO loginRequestDTO = new LoginRequestDTO();
            return View(loginRequestDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            APIResponse aPIResponse = await _IAuth.LoginAsync<APIResponse>(loginRequestDTO);
            if(aPIResponse != null && aPIResponse.IsSuccess)
            {
                LoginResponseDTO model = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(aPIResponse.Result));
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, model.User.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Role, model.User.Role));
                var Principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, Principal);
                HttpContext.Session.SetString(StaticDetails.SessionToken, model.Token);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //TempData["error"] = aPIResponse?.Errors.FirstOrDefault() ?? "Error occurred while creating hotel.";
                ModelState.AddModelError("Errors", aPIResponse?.Errors?.FirstOrDefault() ?? "Invalid login attempt.");

                return View(loginRequestDTO);
            }
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
            if(aPIResponse != null && aPIResponse.IsSuccess)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(StaticDetails.SessionToken, "");
            return RedirectToAction("Index", "Home");
        }

    }
}
