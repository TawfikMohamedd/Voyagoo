using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voyagoo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddHotelRoomTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DoublePrice",
                table: "Hotels",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "DoubleRooms",
                table: "Hotels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "SinglePrice",
                table: "Hotels",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "SingleRooms",
                table: "Hotels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "SuitePrice",
                table: "Hotels",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "SuiteRooms",
                table: "Hotels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TriplePrice",
                table: "Hotels",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TripleRooms",
                table: "Hotels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBYDtncABwNnfFKAve4M589WGlBvSZ1AMvcGvr7YUtvxKlKb/XAy9dDk8UWVlzgcLQ==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoublePrice",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "DoubleRooms",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "SinglePrice",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "SingleRooms",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "SuitePrice",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "SuiteRooms",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "TriplePrice",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "TripleRooms",
                table: "Hotels");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAED5SxjYy2B/Bm0KiavGl4cQ66hDYmYt+se/hdm1f3oAZatMgZPNyjQTsWwoPXNqUWw==");
        }
    }
}
