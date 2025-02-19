using System.Linq.Expressions;
using Book_Your_Hotel.Models;

namespace Book_Your_Hotel.Repositary.IRepositary
{
    public interface IHotelRepo : IMainRepo<Hotels>
    {       
        Task<Hotels> UpdateAsync(Hotels hotels);

    }
}
