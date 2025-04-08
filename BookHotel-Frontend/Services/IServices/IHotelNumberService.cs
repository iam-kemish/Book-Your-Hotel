using BookHotel_Frontend.Models.DTOs;

namespace BookHotel_Frontend.Services.IServices
{
    public interface IHotelNumberService
    {
        Task<T> GetAllAsync<T>( string Token);
        Task<T> GetAsync<T>(int id, string Token);
        Task<T> CreateAsync<T>(HotelNoCreateDTO dto, string Token);
        Task<T> UpdateAsync<T>(HotelNoUpdateDTO dto, string Token);
        Task<T> DeleteAsync<T>(int id, string Token);
    }
}
