using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Book_Your_Hotel.Migrations
{
    /// <inheritdoc />
    public partial class OccupancyColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Occupancy",
                table: "HotelLists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "HotelLists",
                keyColumn: "Id",
                keyValue: 1,
                column: "Occupancy",
                value: 0);

            migrationBuilder.UpdateData(
                table: "HotelLists",
                keyColumn: "Id",
                keyValue: 2,
                column: "Occupancy",
                value: 0);

            migrationBuilder.UpdateData(
                table: "HotelLists",
                keyColumn: "Id",
                keyValue: 3,
                column: "Occupancy",
                value: 0);

            migrationBuilder.UpdateData(
                table: "HotelLists",
                keyColumn: "Id",
                keyValue: 4,
                column: "Occupancy",
                value: 0);

            migrationBuilder.UpdateData(
                table: "HotelLists",
                keyColumn: "Id",
                keyValue: 5,
                column: "Occupancy",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Occupancy",
                table: "HotelLists");
        }
    }
}
