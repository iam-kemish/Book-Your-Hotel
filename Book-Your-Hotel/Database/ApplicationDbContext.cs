using Book_Your_Hotel.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Book_Your_Hotel.Database
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
       
        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<Hotels> HotelLists { get; set; }

        public DbSet<HotelNumbers> HotelNumbers { get; set; }

        public DbSet<LocalUser> LocalUsers { get; set; }    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Hotels>().HasData(
                new Hotels
                {
                    Id = 1,
                    Name = "Luxury Palace",
                    Location = "Kathmandu, Nepal",
                    CreatedOn = new DateTime(2024, 1, 1, 12, 0, 0), // Static DateTime
                    UpdatedOn = new DateTime(2024, 1, 1, 12, 0, 0),
                    ImageUrl = "https://www.dotnetmastery.com/bluevillaimages/villa1.jpg",
                    NumberOfRooms = 150,
                    AvailableRooms = 10,
                    Price = 2000,
                    ContactNumber = "+977-9800000001"
                },
                new Hotels
                {
                    Id = 2,
                    Name = "Everest View Resort",
                    Location = "Solukhumbu, Nepal",
                    CreatedOn = new DateTime(2024, 1, 2, 14, 30, 0),
                    UpdatedOn = new DateTime(2024, 1, 2, 14, 30, 0),
                    ImageUrl = "https://www.dotnetmastery.com/bluevillaimages/villa2.jpg",
                    NumberOfRooms = 80,
                    AvailableRooms = 5,
                    Price = 3690,
                    ContactNumber = "+977-9800000002"
                },
                new Hotels
                {
                    Id = 3,
                    Name = "Himalayan Bliss",
                    Location = "Pokhara, Nepal",
                    CreatedOn = new DateTime(2024, 1, 3, 10, 15, 0),
                    UpdatedOn = new DateTime(2024, 1, 3, 10, 15, 0),
                    ImageUrl = "https://www.dotnetmastery.com/bluevillaimages/villa3.jpg",
                    NumberOfRooms = 1400,
                    AvailableRooms = 15,
                    Price = 1150,
                    ContactNumber = "+977-9800000003"
                },
                new Hotels
                {
                    Id = 4,
                    Name = "Ocean View Hotel",
                    Location = "Goa, India",
                    CreatedOn = new DateTime(2024, 1, 4, 8, 45, 0),
                    UpdatedOn = new DateTime(2024, 1, 4, 8, 45, 0),
                    ImageUrl = "https://www.dotnetmastery.com/bluevillaimages/villa4.jpg",
                    NumberOfRooms = 1200,
                    AvailableRooms = 20,
                    Price = 1180,
                    ContactNumber = "+91-9800000004"
                },
                new Hotels
                {
                    Id = 5,
                    Name = "Desert Rose Inn",
                    Location = "Dubai, UAE",
                    CreatedOn = new DateTime(2024, 1, 5, 17, 20, 0),
                    UpdatedOn = new DateTime(2024, 1, 5, 17, 20, 0),
                    ImageUrl = "https://www.dotnetmastery.com/bluevillaimages/villa5.jpg",
                    NumberOfRooms = 250,
                    AvailableRooms = 2,
                    Price = 1500,
                    ContactNumber = "+971-9800000005"
                }
               
             
               
            );
        }
    }
}
