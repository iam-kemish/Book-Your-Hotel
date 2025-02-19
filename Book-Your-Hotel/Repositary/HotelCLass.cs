using System.Linq.Expressions;
using Book_Your_Hotel.Database;
using Book_Your_Hotel.Models;
using Book_Your_Hotel.Repositary.IRepositary;
using Microsoft.EntityFrameworkCore;

namespace Book_Your_Hotel.Repositary
{
    public class HotelCLass : MainRepoClass<Hotels>,  IHotelRepo
    {
        private readonly ApplicationDbContext _Db;

        public HotelCLass(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _Db = applicationDbContext;
        }
        

        public async Task<Hotels> UpdateAsync(Hotels hotels)
        {
            hotels.UpdatedOn = DateTime.Now;
           _Db.HotelLists.Update(hotels);
           await _Db.SaveChangesAsync();
            return hotels;
        }
    }
}
