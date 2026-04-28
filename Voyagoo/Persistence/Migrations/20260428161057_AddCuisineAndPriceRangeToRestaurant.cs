using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voyagoo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCuisineAndPriceRangeToRestaurant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CuisineType",
                table: "Restaurants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxPrice",
                table: "Restaurants",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinPrice",
                table: "Restaurants",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKizd3juu3Sg/wyKYcZjVLQOABUwXqoSx4Q+diPIX3648GAJU1UeiavuYLI4XA26Ig==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CuisineType",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "MaxPrice",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "MinPrice",
                table: "Restaurants");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHBE1uJmpigiiOq4s26DB7E4dqJm9MDNn0BQZ6sbyH/js76HVjiKVu2zJ+Q310gT7A==");
        }
    }
}
