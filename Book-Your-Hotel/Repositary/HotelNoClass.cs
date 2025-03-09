using Book_Your_Hotel.Database;
using Book_Your_Hotel.Models;

namespace Book_Your_Hotel.Repositary
{
    public class HotelNoClass: MainRepoClass<HotelNumbers>
    {
        private readonly ApplicationDbContext _Db;
        public HotelNoClass(ApplicationDbContext Db): base(Db)
        {
            _Db = Db;
        }
       public async Task<HotelNumbers> UpdateAsync(HotelNumbers hotelNumbers)
        {
            hotelNumbers.LastUpdatedDate = DateTime.Now;
            _Db.HotelNumbers.Update(hotelNumbers);
            await _Db.SaveChangesAsync();
            return hotelNumbers;
        }
    }
}
