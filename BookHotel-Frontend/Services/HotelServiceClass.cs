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
        private readonly IToken token;
        public HotelServiceClass(IHttpClientFactory httpClientFactory, IConfiguration configuration, IToken token) : base(httpClientFactory, token)
        {
            _httpClientFactory = httpClientFactory;
            HotelUrl = configuration.GetValue<string>("ServiceUrls:BookHotelApi");
        }

        public Task<T> CreateAsync<T>(HotelCreateDTO dto)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = dto,
                Url = HotelUrl+ "api/HotelLists",
                
                ContentType = StaticDetails.ContentType.MultipartFormData
            });
        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.DELETE,
                Url = HotelUrl+ "api/HotelLists/" + id,
              
            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = HotelUrl+ "api/HotelLists/",
              
            });
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = HotelUrl+ "api/HotelLists/" + id,
              
            });
        }

        public Task<T> UpdateAsync<T>(HotelUpdateDTO dto)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.PUT,
                Data = dto,
                Url = HotelUrl+ "api/HotelLists/" + dto.Id,             
                ContentType = StaticDetails.ContentType.MultipartFormData
            });
        }
    }
}
