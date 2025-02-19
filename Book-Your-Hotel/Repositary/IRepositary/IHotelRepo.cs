using System.Linq.Expressions;
using Book_Your_Hotel.Models;

namespace Book_Your_Hotel.Repositary.IRepositary
{
    public interface IHotelRepo
    {
        Task<List<Hotels>> GetAllAsync(Expression<Func<Hotels, bool>> filter = null);
        Task<Hotels> GetAsync(Expression<Func<Hotels, bool>> filter = null, bool tracked = true);
        Task CreateAsync(Hotels hotels);
        Task RemoveAsync(Hotels hotels);

        Task UpdateAsync(Hotels hotels);

        Task SaveAsync();

    }
}
