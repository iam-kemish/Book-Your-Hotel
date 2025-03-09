using Book_Your_Hotel.Models;

namespace Book_Your_Hotel.Repositary.IRepositary
{
    public interface IHotelNoRepo: IMainRepo<HotelNumbers>
    {
        Task<HotelNumbers> UpdateAsync(HotelNumbers HotelNumbers);
    }
}
