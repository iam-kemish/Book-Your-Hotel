using BookHotel_Frontend.Models;

namespace BookHotel_Frontend.Services.IServices
{
    public interface IBaseService
    {
        APIResponse APIResponse { get; }

        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}
