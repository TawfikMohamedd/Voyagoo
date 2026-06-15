using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voyagoo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddHotelDiscountAndServiceCharge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "Hotels",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceCharge",
                table: "Hotels",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 15, 4, 23, 0, 71, DateTimeKind.Utc).AddTicks(3839), "AQAAAAIAAYagAAAAEGnPEdygUUd5D1kQMkyQEhP3NEt+8hWzNfMwXZ71KpqfIkhT9+AIg9jMAYpV6V4jsg==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "ServiceCharge",
                table: "Hotels");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 14, 8, 58, 21, 585, DateTimeKind.Utc).AddTicks(8883), "AQAAAAIAAYagAAAAEFJ86ACh9RUNLZCmnxkvv7Snr3CGglwMag47wN6R0JzqzpXUyLfxl1B/qTvrokG79A==" });
        }
    }
}
