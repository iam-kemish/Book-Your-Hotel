using BookHotel_Frontend.Models;

namespace BookHotel_Frontend.Services.IServices
{
    public interface IBaseService
    {
        APIResponse APIResponse { get; set; }

        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}
