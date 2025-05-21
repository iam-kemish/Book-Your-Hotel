using BookHotel_Frontend.Models.DTOs;

namespace BookHotel_Frontend.Services.IServices
{
    public interface IHotelNumberService
    {
        Task<T> GetAllAsync<T>( );
        Task<T> GetAsync<T>(int id );
        Task<T> CreateAsync<T>(HotelNoCreateDTO dto );
        Task<T> UpdateAsync<T>(HotelNoUpdateDTO dto );
        Task<T> DeleteAsync<T>(int id );
    }
}
