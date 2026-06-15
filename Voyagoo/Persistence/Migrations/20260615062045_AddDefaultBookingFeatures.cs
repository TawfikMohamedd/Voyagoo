using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Voyagoo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultBookingFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 15, 6, 20, 45, 420, DateTimeKind.Utc).AddTicks(8216), "AQAAAAIAAYagAAAAECJSluIRJEdMAJU4L5FUXsoGPNNNBsgZEi0XX6m7Y+Cuhm8mz9c2UCIwqPHNvnwgpQ==" });

            migrationBuilder.InsertData(
                table: "BookingFeatures",
                columns: new[] { "Id", "Icon", "Name" },
                values: new object[,]
                {
                    { 1001, "full-board-icon", "Full Board" },
                    { 1002, "half-board-icon", "Half Board" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BookingFeatures",
                keyColumn: "Id",
                keyValue: 1001);

            migrationBuilder.DeleteData(
                table: "BookingFeatures",
                keyColumn: "Id",
                keyValue: 1002);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 15, 5, 15, 1, 32, DateTimeKind.Utc).AddTicks(5321), "AQAAAAIAAYagAAAAEFLGj0eK5R6whA1PURN+VT4WepjxHx9Jy+0+LTan0pSyjeJg1cHrfcxe4GqSIToF2w==" });
        }
    }
}
