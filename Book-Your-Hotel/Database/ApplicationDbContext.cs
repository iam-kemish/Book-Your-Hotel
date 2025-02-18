using Book_Your_Hotel.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Book_Your_Hotel.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Hotels> HotelLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hotels>().HasData(
                new Hotels
                {
                    Id = 1,
                    Name = "Luxury Palace",
                    Location = "Kathmandu, Nepal",
                    CreatedOn = new DateTime(2024, 1, 1, 12, 0, 0), // Static DateTime
                    UpdatedOn = new DateTime(2024, 1, 1, 12, 0, 0),
                    ImageUrl = "https://example.com/luxury-palace.jpg",
                    NumberOfRooms = 150,
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
                    ImageUrl = "https://example.com/everest-view.jpg",
                    NumberOfRooms = 80,
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
                    ImageUrl = "https://example.com/himalayan-bliss.jpg",
                    NumberOfRooms = 1400,
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
                    ImageUrl = "https://example.com/ocean-view.jpg",
                    NumberOfRooms = 1200,
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
                    ImageUrl = "https://example.com/desert-rose.jpg",
                    NumberOfRooms = 250,
                    Price = 1500,
                    ContactNumber = "+971-9800000005"
                },
                new Hotels
                {
                    Id = 6,
                    Name = "Alpine Lodge",
                    Location = "Zermatt, Switzerland",
                    CreatedOn = new DateTime(2024, 1, 6, 11, 10, 0),
                    UpdatedOn = new DateTime(2024, 1, 6, 11, 10, 0),
                    ImageUrl = "https://example.com/alpine-lodge.jpg",
                    NumberOfRooms = 90,
                    Price = 1400,
                    ContactNumber = "+41-9800000006"
                },
                new Hotels
                {
                    Id = 7,
                    Name = "Skyline Suites",
                    Location = "New York, USA",
                    CreatedOn = new DateTime(2024, 1, 7, 9, 55, 0),
                    UpdatedOn = new DateTime(2024, 1, 7, 9, 55, 0),
                    ImageUrl = "https://example.com/skyline-suites.jpg",
                    NumberOfRooms = 300,
                    Price = 1600,
                    ContactNumber = "+1-9800000007"
                },
                new Hotels
                {
                    Id = 8,
                    Name = "Serene Beach Resort",
                    Location = "Bali, Indonesia",
                    CreatedOn = new DateTime(2024, 1, 8, 16, 40, 0),
                    UpdatedOn = new DateTime(2024, 1, 8, 16, 40, 0),
                    ImageUrl = "https://example.com/serene-beach.jpg",
                    NumberOfRooms = 180,
                    Price = 1220,
                    ContactNumber = "+62-9800000008"
                }
            );
        }
    }
}
