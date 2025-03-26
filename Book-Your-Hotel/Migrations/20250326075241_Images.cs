using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Book_Your_Hotel.Migrations
{
    /// <inheritdoc />
    public partial class Images : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "HotelLists",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.UpdateData(
                table: "HotelLists",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://www.dotnetmastery.com/bluevillaimages/villa1.jpg");

            migrationBuilder.UpdateData(
                table: "HotelLists",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://www.dotnetmastery.com/bluevillaimages/villa2.jpg");

            migrationBuilder.UpdateData(
                table: "HotelLists",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://www.dotnetmastery.com/bluevillaimages/villa3.jpg");

            migrationBuilder.UpdateData(
                table: "HotelLists",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "https://www.dotnetmastery.com/bluevillaimages/villa4.jpg");

            migrationBuilder.UpdateData(
                table: "HotelLists",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: "https://www.dotnetmastery.com/bluevillaimages/villa5.jpg");

            migrationBuilder.UpdateData(
                table: "HotelLists",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImageUrl",
                value: "https://unsplash.com/photos/blue-body-of-water-in-front-of-building-near-trees-during-nighttime-M7GddPqJowg");

            migrationBuilder.UpdateData(
                table: "HotelLists",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImageUrl",
                value: "https://unsplash.com/photos/gray-table-lamp-beside-white-bed-pillow-uocSnWMhnAs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "HotelLists",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://example.com/luxury-palace.jpg");

            migrationBuilder.UpdateData(
                table: "HotelLists",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://example.com/everest-view.jpg");

            migrationBuilder.UpdateData(
                table: "HotelLists",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://example.com/himalayan-bliss.jpg");

            migrationBuilder.UpdateData(
                table: "HotelLists",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "https://example.com/ocean-view.jpg");

            migrationBuilder.UpdateData(
                table: "HotelLists",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: "https://example.com/desert-rose.jpg");

            migrationBuilder.UpdateData(
                table: "HotelLists",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImageUrl",
                value: "https://example.com/alpine-lodge.jpg");

            migrationBuilder.UpdateData(
                table: "HotelLists",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImageUrl",
                value: "https://example.com/skyline-suites.jpg");

            migrationBuilder.InsertData(
                table: "HotelLists",
                columns: new[] { "Id", "ContactNumber", "CreatedOn", "ImageUrl", "Location", "Name", "NumberOfRooms", "Price", "UpdatedOn" },
                values: new object[] { 8, "+62-9800000008", new DateTime(2024, 1, 8, 16, 40, 0, 0, DateTimeKind.Unspecified), "https://example.com/serene-beach.jpg", "Bali, Indonesia", "Serene Beach Resort", 180, 1220, new DateTime(2024, 1, 8, 16, 40, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
