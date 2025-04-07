﻿using BookHotel_Frontend.Models;
using BookHotel_Frontend.Models.DTOs;
using BookHotel_Frontend.Services.IServices;
using Microsoft.AspNetCore.Authentication;
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            APIResponse aPIResponse = await _IAuth.LoginAsync<APIResponse>(loginRequestDTO);
            if(aPIResponse != null && aPIResponse.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(aPIResponse.Result));
                HttpContext.Session.SetString("JWTToken", model.Token);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("ErrorMessage", aPIResponse.Errors.FirstOrDefault());
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
            if(aPIResponse == null && aPIResponse.IsSuccess)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString("JWTToken", "");
            return RedirectToAction("Index", "Home");
        }

    }
}
