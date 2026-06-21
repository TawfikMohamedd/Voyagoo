using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voyagoo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatePass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 20, 1, 45, 1, 63, DateTimeKind.Utc).AddTicks(3125), "AQAAAAIAAYagAAAAEIZw9IN4HQWMFEUA78WxHW0qnqkeB0i4ejyWsOouyS4Z8ZKGzxB4qbeHDeO63ETvhA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 19, 23, 52, 15, 888, DateTimeKind.Utc).AddTicks(8335), "AQAAAAIAAYagAAAAEBg8OWE28f4whG1oDWVMrrBONPZhsIuj1Gpzs0jSpTExWed6z00L0ubZPXL369aa7Q==" });
        }
    }
}
