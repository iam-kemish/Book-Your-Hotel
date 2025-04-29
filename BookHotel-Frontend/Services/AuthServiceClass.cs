
using BookHotel_Frontend.Models;
using BookHotel_Frontend.Models.DTOs;
using BookHotel_Frontend.Services.IServices;
using BookHotel_Utilities;

namespace BookHotel_Frontend.Services
{
    public class AuthServiceClass: BaseServiceClass, IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string HotelNoUrl;
        public AuthServiceClass(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            HotelNoUrl = configuration.GetValue<string>("ServiceUrls:BookHotelApi");

        }
   
        public Task<T> LoginAsync<T>(LoginRequestDTO loginRequestDTO)
        {
            return SendAsync<T>(
             new ApiRequest
             {
                 ApiType = StaticDetails.ApiType.POST,
                 Data = loginRequestDTO,
                 Url = HotelNoUrl+"api/Users/Login/"
             }

             );
        }

        public Task<T> RegisterAsync<T>(RegisterRequestDTO registerRequestDTO)
        {
            return SendAsync<T>(
             new ApiRequest
             {
                 ApiType = StaticDetails.ApiType.POST,
                 Data = registerRequestDTO,
                 Url = HotelNoUrl+ "api/Users/Register/"
             }

             );
        }
    }
}
