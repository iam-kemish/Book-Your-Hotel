using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Book_Your_Hotel.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedLocalUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HotelLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfRooms = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LocalUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HotelNumbers",
                columns: table => new
                {
                    HotelNumber = table.Column<int>(type: "int", nullable: false),
                    HotelID = table.Column<int>(type: "int", nullable: false),
                    SpecialDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelNumbers", x => x.HotelNumber);
                    table.ForeignKey(
                        name: "FK_HotelNumbers_HotelLists_HotelID",
                        column: x => x.HotelID,
                        principalTable: "HotelLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "HotelLists",
                columns: new[] { "Id", "ContactNumber", "CreatedOn", "ImageUrl", "Location", "Name", "NumberOfRooms", "Price", "UpdatedOn" },
                values: new object[,]
                {
                    { 1, "+977-9800000001", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), "https://www.dotnetmastery.com/bluevillaimages/villa1.jpg", "Kathmandu, Nepal", "Luxury Palace", 150, 2000, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "+977-9800000002", new DateTime(2024, 1, 2, 14, 30, 0, 0, DateTimeKind.Unspecified), "https://www.dotnetmastery.com/bluevillaimages/villa2.jpg", "Solukhumbu, Nepal", "Everest View Resort", 80, 3690, new DateTime(2024, 1, 2, 14, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "+977-9800000003", new DateTime(2024, 1, 3, 10, 15, 0, 0, DateTimeKind.Unspecified), "https://www.dotnetmastery.com/bluevillaimages/villa3.jpg", "Pokhara, Nepal", "Himalayan Bliss", 1400, 1150, new DateTime(2024, 1, 3, 10, 15, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "+91-9800000004", new DateTime(2024, 1, 4, 8, 45, 0, 0, DateTimeKind.Unspecified), "https://www.dotnetmastery.com/bluevillaimages/villa4.jpg", "Goa, India", "Ocean View Hotel", 1200, 1180, new DateTime(2024, 1, 4, 8, 45, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "+971-9800000005", new DateTime(2024, 1, 5, 17, 20, 0, 0, DateTimeKind.Unspecified), "https://www.dotnetmastery.com/bluevillaimages/villa5.jpg", "Dubai, UAE", "Desert Rose Inn", 250, 1500, new DateTime(2024, 1, 5, 17, 20, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_HotelNumbers_HotelID",
                table: "HotelNumbers",
                column: "HotelID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HotelNumbers");

            migrationBuilder.DropTable(
                name: "LocalUsers");

            migrationBuilder.DropTable(
                name: "HotelLists");
        }
    }
}
