﻿// <auto-generated />
using System;
using Book_Your_Hotel.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Book_Your_Hotel.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250218113913_Initial1")]
    partial class Initial1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Book_Your_Hotel.Models.Hotels", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ContactNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberOfRooms")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("HotelLists");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ContactNumber = "+977-9800000001",
                            CreatedOn = new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "https://example.com/luxury-palace.jpg",
                            Location = "Kathmandu, Nepal",
                            Name = "Luxury Palace",
                            NumberOfRooms = 150,
                            Price = 2000,
                            UpdatedOn = new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 2,
                            ContactNumber = "+977-9800000002",
                            CreatedOn = new DateTime(2024, 1, 2, 14, 30, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "https://example.com/everest-view.jpg",
                            Location = "Solukhumbu, Nepal",
                            Name = "Everest View Resort",
                            NumberOfRooms = 80,
                            Price = 3690,
                            UpdatedOn = new DateTime(2024, 1, 2, 14, 30, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 3,
                            ContactNumber = "+977-9800000003",
                            CreatedOn = new DateTime(2024, 1, 3, 10, 15, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "https://example.com/himalayan-bliss.jpg",
                            Location = "Pokhara, Nepal",
                            Name = "Himalayan Bliss",
                            NumberOfRooms = 1400,
                            Price = 1150,
                            UpdatedOn = new DateTime(2024, 1, 3, 10, 15, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 4,
                            ContactNumber = "+91-9800000004",
                            CreatedOn = new DateTime(2024, 1, 4, 8, 45, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "https://example.com/ocean-view.jpg",
                            Location = "Goa, India",
                            Name = "Ocean View Hotel",
                            NumberOfRooms = 1200,
                            Price = 1180,
                            UpdatedOn = new DateTime(2024, 1, 4, 8, 45, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 5,
                            ContactNumber = "+971-9800000005",
                            CreatedOn = new DateTime(2024, 1, 5, 17, 20, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "https://example.com/desert-rose.jpg",
                            Location = "Dubai, UAE",
                            Name = "Desert Rose Inn",
                            NumberOfRooms = 250,
                            Price = 1500,
                            UpdatedOn = new DateTime(2024, 1, 5, 17, 20, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 6,
                            ContactNumber = "+41-9800000006",
                            CreatedOn = new DateTime(2024, 1, 6, 11, 10, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "https://example.com/alpine-lodge.jpg",
                            Location = "Zermatt, Switzerland",
                            Name = "Alpine Lodge",
                            NumberOfRooms = 90,
                            Price = 1400,
                            UpdatedOn = new DateTime(2024, 1, 6, 11, 10, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 7,
                            ContactNumber = "+1-9800000007",
                            CreatedOn = new DateTime(2024, 1, 7, 9, 55, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "https://example.com/skyline-suites.jpg",
                            Location = "New York, USA",
                            Name = "Skyline Suites",
                            NumberOfRooms = 300,
                            Price = 1600,
                            UpdatedOn = new DateTime(2024, 1, 7, 9, 55, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 8,
                            ContactNumber = "+62-9800000008",
                            CreatedOn = new DateTime(2024, 1, 8, 16, 40, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "https://example.com/serene-beach.jpg",
                            Location = "Bali, Indonesia",
                            Name = "Serene Beach Resort",
                            NumberOfRooms = 180,
                            Price = 1220,
                            UpdatedOn = new DateTime(2024, 1, 8, 16, 40, 0, 0, DateTimeKind.Unspecified)
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
