using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Book_Your_Hotel.Migrations
{
    /// <inheritdoc />
    public partial class InitialOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailableRooms",
                table: "HotelLists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "HotelLists",
                keyColumn: "Id",
                keyValue: 1,
                column: "AvailableRooms",
                value: 10);

            migrationBuilder.UpdateData(
                table: "HotelLists",
                keyColumn: "Id",
                keyValue: 2,
                column: "AvailableRooms",
                value: 5);

            migrationBuilder.UpdateData(
                table: "HotelLists",
                keyColumn: "Id",
                keyValue: 3,
                column: "AvailableRooms",
                value: 15);

            migrationBuilder.UpdateData(
                table: "HotelLists",
                keyColumn: "Id",
                keyValue: 4,
                column: "AvailableRooms",
                value: 20);

            migrationBuilder.UpdateData(
                table: "HotelLists",
                keyColumn: "Id",
                keyValue: 5,
                column: "AvailableRooms",
                value: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableRooms",
                table: "HotelLists");
        }
    }
}
