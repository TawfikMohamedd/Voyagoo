using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voyagoo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTableCapacitiesToRestaurant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TablesForFour",
                table: "Restaurants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TablesForSix",
                table: "Restaurants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TablesForTwo",
                table: "Restaurants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECxgsRu1yutrE6uFU14ozl3vXMuytu5fg3McNVB2DzOm5+J/iKy6FGhwcikGG8agsw==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TablesForFour",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "TablesForSix",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "TablesForTwo",
                table: "Restaurants");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKizd3juu3Sg/wyKYcZjVLQOABUwXqoSx4Q+diPIX3648GAJU1UeiavuYLI4XA26Ig==");
        }
    }
}
