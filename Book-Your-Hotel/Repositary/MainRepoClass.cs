using Book_Your_Hotel.Database;
using Book_Your_Hotel.Models;
using System.Linq.Expressions;
using Book_Your_Hotel.Repositary.IRepositary;
using Microsoft.EntityFrameworkCore;

namespace Book_Your_Hotel.Repositary
{
    public class MainRepoClass<T> : IMainRepo<T> where T : class
    {
        private readonly ApplicationDbContext _Db;
        internal DbSet<T> DbSet;

        public MainRepoClass(ApplicationDbContext applicationDbContext)
        {
            _Db = applicationDbContext;
            this.DbSet = _Db.Set<T>();
        }
        public async Task CreateAsync(T item)
        {
          await DbSet.AddAsync(item);
            await SaveAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true)
        {
            IQueryable<T> Query = DbSet;

            if (!tracked)
            {
                Query.AsNoTracking();
            }
            if (filter != null)
            {
                Query = Query.Where(filter);
            }
            return await Query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> Query = DbSet;
            if (filter != null)
            {
                Query = Query.Where(filter);
            };
            return await Query.ToListAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            DbSet.Remove(entity);
            await SaveAsync();
        }
       
        public async Task SaveAsync()
        {
            await _Db.SaveChangesAsync();
        }

    }
}
