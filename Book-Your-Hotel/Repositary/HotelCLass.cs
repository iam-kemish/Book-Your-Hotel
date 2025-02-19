using System.Linq.Expressions;
using Book_Your_Hotel.Database;
using Book_Your_Hotel.Models;
using Book_Your_Hotel.Repositary.IRepositary;
using Microsoft.EntityFrameworkCore;

namespace Book_Your_Hotel.Repositary
{
    public class HotelCLass : IHotelRepo
    {
        private readonly ApplicationDbContext _Db;

        public HotelCLass(ApplicationDbContext applicationDbContext)
        {
            _Db = applicationDbContext;
        }
        public async Task CreateAsync(Hotels hotels)
        {
          await _Db.HotelLists.AddAsync(hotels);
            await SaveAsync();
        }

        public async Task<Hotels> GetAsync(Expression<Func<Hotels, bool>> filter = null, bool tracked = true)
        {
            IQueryable<Hotels> Query = _Db.HotelLists;

            if (!tracked)
            {
                Query.AsNoTracking();
            }
            if(filter != null)
            {
                Query = Query.Where(filter);
            }
            return await Query.FirstOrDefaultAsync();
        }

        public async Task<List<Hotels>> GetAllAsync(Expression<Func<Hotels, bool>> filter = null)
        {
            IQueryable<Hotels> Query = _Db.HotelLists;
            if (filter != null)
            {
                Query = Query.Where(filter);
            };
            return await Query.ToListAsync();
        }

        public async Task RemoveAsync(Hotels hotels)
        {
             _Db.HotelLists.Remove(hotels);
            await SaveAsync();
        }
        public async Task UpdateAsync(Hotels hotels)
        {
           _Db.HotelLists.Update(hotels);
            await SaveAsync();
           
        }
        public async Task SaveAsync()
        {
            await _Db.SaveChangesAsync();
        }

       
    }
}
