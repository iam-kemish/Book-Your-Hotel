using BookHotel_Frontend.Models;
using BookHotel_Frontend.Models.DTOs;
using BookHotel_Frontend.Services.IServices;
using BookHotel_Utilities;

namespace BookHotel_Frontend.Services
{
    public class HotelNumberServiceClass : BaseServiceClass, IHotelNumberService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string HotelNoUrl;
        public HotelNumberServiceClass(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            HotelNoUrl = configuration.GetValue<string>("ServiceUrls:BookHotelApi");

        }

        public Task<T> CreateAsync<T>(HotelNoCreateDTO dto, string Token)
        {
            return SendAsync<T>(
                new ApiRequest
                {
                    ApiType = StaticDetails.ApiType.POST,
                    Data = dto,
                    Url = HotelNoUrl + "api/HotelNumbers/",
                    token = Token
                }

                );
        }

        public Task<T> DeleteAsync<T>(int id, string Token)
        {
            return SendAsync<T>(
                new ApiRequest
                {
                    ApiType = StaticDetails.ApiType.DELETE,
                    Url = HotelNoUrl + "api/HotelNumbers/" + id,
                    token = Token
                }

                );
        }

        public Task<T> GetAllAsync<T>(string Token)
        {
            return SendAsync<T>(
                 new ApiRequest
                 {
                     ApiType = StaticDetails.ApiType.GET,                   
                     Url = HotelNoUrl + "api/HotelNumbers/",
                     token = Token
                 }

                 );
        }

        public Task<T> GetAsync<T>(int id, string Token)
        {
            return SendAsync<T>(
                new ApiRequest
                {
                    ApiType = StaticDetails.ApiType.GET,
                    Url = HotelNoUrl + "api/HotelNumbers/" + id,
                    token = Token
                }
                );
        }

        public Task<T> UpdateAsync<T>(HotelNoUpdateDTO dto, string Token)
        {
            return SendAsync<T>(new ApiRequest 
            { 
                ApiType = StaticDetails.ApiType.PUT, 
                Data = dto,
                Url = HotelNoUrl + "api/HotelNumbers/" + dto.HotelNumber,
                token = Token
            });
        }
    }
}
