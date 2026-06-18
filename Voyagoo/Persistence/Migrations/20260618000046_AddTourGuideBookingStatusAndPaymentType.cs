using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voyagoo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTourGuideBookingStatusAndPaymentType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PricePerDay",
                table: "TourGuides",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "TourGuideBookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "TourGuideBookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 18, 0, 0, 44, 937, DateTimeKind.Utc).AddTicks(1212), "AQAAAAIAAYagAAAAEAqD4hZ151FW3lPRYl1rQ/UMWyu+dd2tiy5GvoDoMkuEZvyyj9x0oJMaoiV+NYLEhA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "TourGuideBookings");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "TourGuideBookings");

            migrationBuilder.AlterColumn<decimal>(
                name: "PricePerDay",
                table: "TourGuides",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 17, 23, 12, 39, 575, DateTimeKind.Utc).AddTicks(9081), "AQAAAAIAAYagAAAAEF1O2tP0onhaCp3K2FbYjkW4wqoqitEeexewZsYG5czr1PO6qdKcXp2oO3YWq8h4Ew==" });
        }
    }
}
