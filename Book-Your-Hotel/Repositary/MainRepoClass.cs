using Book_Your_Hotel.Database;
using Book_Your_Hotel.Models;
using System.Linq.Expressions;
using Book_Your_Hotel.Repositary.IRepositary;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null)
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
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    Query = Query.Include(includeProp);
                }
            }
            return await Query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, int PageSize = 3, int PageNumber = 1)
        {
            IQueryable<T> Query = DbSet;
            if (filter != null)
            {
                Query = Query.Where(filter);
            };
            if(PageSize > 0)
            {
                if(PageSize > 100)
                {
                    //defaulting page size as 100
                    PageSize = 100;
                }
                //suppose if page size is 3 and pagenum is 1, a/c to below formula (3 * (1-1).take(3)), it means we skip 0 records and display 3 records,
                //suppose if page size is 3 and pagenum is 2, a/c to below formula (3 * (2-1).take(3)), it means we skip 3 records and display next 3 records.
                Query = Query.Skip(PageSize * (PageNumber - 1)).Take(PageSize);
            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    Query = Query.Include(includeProp);
                }
            }
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
