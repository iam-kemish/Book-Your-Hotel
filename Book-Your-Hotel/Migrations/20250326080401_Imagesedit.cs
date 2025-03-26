using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Book_Your_Hotel.Migrations
{
    /// <inheritdoc />
    public partial class Imagesedit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "HotelLists",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "HotelLists",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "HotelLists",
                keyColumn: "Id",
                keyValue: 7);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "HotelLists",
                columns: new[] { "Id", "ContactNumber", "CreatedOn", "ImageUrl", "Location", "Name", "NumberOfRooms", "Price", "UpdatedOn" },
                values: new object[,]
                {
                    { 5, "+971-9800000005", new DateTime(2024, 1, 5, 17, 20, 0, 0, DateTimeKind.Unspecified), "https://www.dotnetmastery.com/bluevillaimages/villa5.jpg", "Dubai, UAE", "Desert Rose Inn", 250, 1500, new DateTime(2024, 1, 5, 17, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "+41-9800000006", new DateTime(2024, 1, 6, 11, 10, 0, 0, DateTimeKind.Unspecified), "https://unsplash.com/photos/blue-body-of-water-in-front-of-building-near-trees-during-nighttime-M7GddPqJowg", "Zermatt, Switzerland", "Alpine Lodge", 90, 1400, new DateTime(2024, 1, 6, 11, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 7, "+1-9800000007", new DateTime(2024, 1, 7, 9, 55, 0, 0, DateTimeKind.Unspecified), "https://unsplash.com/photos/gray-table-lamp-beside-white-bed-pillow-uocSnWMhnAs", "New York, USA", "Skyline Suites", 300, 1600, new DateTime(2024, 1, 7, 9, 55, 0, 0, DateTimeKind.Unspecified) }
                });
        }
    }
}
