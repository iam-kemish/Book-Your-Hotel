using Book_Your_Hotel.Models;
using System.Linq.Expressions;

namespace Book_Your_Hotel.Repositary.IRepositary
{
    public interface IMainRepo<T > where T : class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, int PageSize = 3, int PageNumber = 1);
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        Task CreateAsync(T item);
        Task RemoveAsync(T item);
    }
}
