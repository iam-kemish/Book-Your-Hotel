using BookHotel_Frontend.Models.DTOs;

namespace BookHotel_Frontend.Services.IServices
{
    public interface IHotelService
    {
        Task<T> GetAllAsync<T>( string Token);
        Task<T> GetAsync<T>(int id, string Token);
        Task<T> CreateAsync<T>(HotelCreateDTO dto, string Token);
        Task<T> UpdateAsync<T>(HotelUpdateDTO dto, string Token);
        Task<T> DeleteAsync<T>(int id, string Token);
    }
}
