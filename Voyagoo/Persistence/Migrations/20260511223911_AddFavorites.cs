using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voyagoo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFavorites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: true),
                    TourGuideId = table.Column<int>(type: "int", nullable: true),
                    AttractionId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favorites_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorites_Attractions_AttractionId",
                        column: x => x.AttractionId,
                        principalTable: "Attractions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Favorites_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Favorites_TourGuides_TourGuideId",
                        column: x => x.TourGuideId,
                        principalTable: "TourGuides",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJQhaaNVP5sgBngFLZ/AUUTsvEq3cwvzBr0FqP6mQkUGM+q544JCLSQjReHTNqcmkA==");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_AttractionId",
                table: "Favorites",
                column: "AttractionId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_RestaurantId",
                table: "Favorites",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_TourGuideId",
                table: "Favorites",
                column: "TourGuideId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserId_AttractionId",
                table: "Favorites",
                columns: new[] { "UserId", "AttractionId" },
                unique: true,
                filter: "[AttractionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserId_RestaurantId",
                table: "Favorites",
                columns: new[] { "UserId", "RestaurantId" },
                unique: true,
                filter: "[RestaurantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserId_TourGuideId",
                table: "Favorites",
                columns: new[] { "UserId", "TourGuideId" },
                unique: true,
                filter: "[TourGuideId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELBOfACpIoVSizaJWCvNEQdtY0E3826wNJscW08CWYv/GIlJlUmJlE8akujZR4MzNw==");
        }
    }
}
