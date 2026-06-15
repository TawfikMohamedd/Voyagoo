using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voyagoo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddHotelBookingFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookingFeatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingFeatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HotelBookingFeatures",
                columns: table => new
                {
                    HotelId = table.Column<int>(type: "int", nullable: false),
                    BookingFeatureId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelBookingFeatures", x => new { x.HotelId, x.BookingFeatureId });
                    table.ForeignKey(
                        name: "FK_HotelBookingFeatures_BookingFeatures_BookingFeatureId",
                        column: x => x.BookingFeatureId,
                        principalTable: "BookingFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HotelBookingFeatures_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 15, 5, 15, 1, 32, DateTimeKind.Utc).AddTicks(5321), "AQAAAAIAAYagAAAAEFLGj0eK5R6whA1PURN+VT4WepjxHx9Jy+0+LTan0pSyjeJg1cHrfcxe4GqSIToF2w==" });

            migrationBuilder.CreateIndex(
                name: "IX_HotelBookingFeatures_BookingFeatureId",
                table: "HotelBookingFeatures",
                column: "BookingFeatureId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HotelBookingFeatures");

            migrationBuilder.DropTable(
                name: "BookingFeatures");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 15, 4, 23, 0, 71, DateTimeKind.Utc).AddTicks(3839), "AQAAAAIAAYagAAAAEGnPEdygUUd5D1kQMkyQEhP3NEt+8hWzNfMwXZ71KpqfIkhT9+AIg9jMAYpV6V4jsg==" });
        }
    }
}
