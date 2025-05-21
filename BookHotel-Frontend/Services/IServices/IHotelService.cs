using BookHotel_Frontend.Models.DTOs;

namespace BookHotel_Frontend.Services.IServices
{
    public interface IHotelService
    {
        Task<T> GetAllAsync<T>( );
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(HotelCreateDTO dto);
        Task<T> UpdateAsync<T>(HotelUpdateDTO dto);
        Task<T> DeleteAsync<T>(int id);
    }
}
