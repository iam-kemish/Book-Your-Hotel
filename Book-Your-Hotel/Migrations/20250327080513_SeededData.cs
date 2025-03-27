using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Book_Your_Hotel.Migrations
{
    /// <inheritdoc />
    public partial class SeededData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "HotelLists",
                columns: new[] { "Id", "ContactNumber", "CreatedOn", "ImageUrl", "Location", "Name", "NumberOfRooms", "Price", "UpdatedOn" },
                values: new object[] { 5, "+971-9800000005", new DateTime(2024, 1, 5, 17, 20, 0, 0, DateTimeKind.Unspecified), "https://www.dotnetmastery.com/bluevillaimages/villa5.jpg", "Dubai, UAE", "Desert Rose Inn", 250, 1500, new DateTime(2024, 1, 5, 17, 20, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "HotelLists",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
