using BookHotel_Frontend.Models;
using BookHotel_Frontend.Models.DTOs;
using BookHotel_Frontend.Services.IServices;
using BookHotel_Utilities;

namespace BookHotel_Frontend.Services
{
    public class HotelServiceClass : BaseServiceClass, IHotelService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string HotelUrl;
        public HotelServiceClass(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            HotelUrl = configuration.GetValue<string>("ServiceUrls:BookHotelApi");
        }

        public Task<T> CreateAsync<T>(HotelCreateDTO dto, string Token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = dto,
                Url = HotelUrl+ "api/HotelLists",
                token = Token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string Token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.DELETE,
                Url = HotelUrl + "api/HotelLists/" + id,
                token = Token
            });
        }

        public Task<T> GetAllAsync<T>(string Token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = HotelUrl + "api/HotelLists/",
                token = Token
            });
        }

        public Task<T> GetAsync<T>(int id, string Token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = HotelUrl + "api/HotelLists/" + id,
                token = Token
            });
        }

        public Task<T> UpdateAsync<T>(HotelUpdateDTO dto, string Token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.PUT,
                Data = dto,
                Url = HotelUrl + "api/HotelLists/" + dto.Id,
                token = Token
            });
        }
    }
}
