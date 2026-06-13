using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voyagoo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddHotelFavorite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HotelId",
                table: "Favorites",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELX2bVi4NcnLvQnGHP/c5JSZmempgtGnt7Hm345CrbgH2HzJgBDCK4mOCyqrhmg5jA==");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_HotelId",
                table: "Favorites",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserId_HotelId",
                table: "Favorites",
                columns: new[] { "UserId", "HotelId" },
                unique: true,
                filter: "[HotelId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Hotels_HotelId",
                table: "Favorites",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Hotels_HotelId",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_HotelId",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_UserId_HotelId",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "Favorites");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBYDtncABwNnfFKAve4M589WGlBvSZ1AMvcGvr7YUtvxKlKb/XAy9dDk8UWVlzgcLQ==");
        }
    }
}
