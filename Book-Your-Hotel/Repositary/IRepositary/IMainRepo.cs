using Book_Your_Hotel.Models;
using System.Linq.Expressions;

namespace Book_Your_Hotel.Repositary.IRepositary
{
    public interface IMainRepo<T > where T : class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true);
        Task CreateAsync(T item);
        Task RemoveAsync(T item);
    }
}
