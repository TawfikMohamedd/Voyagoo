using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voyagoo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBudgetPlanning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BudgetPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TotalBudget = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    NumberOfDays = table.Column<int>(type: "int", nullable: false),
                    HotelBudget = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    RestaurantBudget = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    AttractionBudget = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    HotelId = table.Column<int>(type: "int", nullable: true),
                    HotelNameSnapshot = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    HotelPriceSnapshot = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetPlans_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetPlans_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "BudgetPlanAttractions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BudgetPlanId = table.Column<int>(type: "int", nullable: false),
                    AttractionId = table.Column<int>(type: "int", nullable: true),
                    AttractionNameSnapshot = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TicketPriceSnapshot = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetPlanAttractions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetPlanAttractions_Attractions_AttractionId",
                        column: x => x.AttractionId,
                        principalTable: "Attractions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BudgetPlanAttractions_BudgetPlans_BudgetPlanId",
                        column: x => x.BudgetPlanId,
                        principalTable: "BudgetPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BudgetPlanRestaurants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BudgetPlanId = table.Column<int>(type: "int", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: true),
                    RestaurantNameSnapshot = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EstimatedPriceSnapshot = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetPlanRestaurants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetPlanRestaurants_BudgetPlans_BudgetPlanId",
                        column: x => x.BudgetPlanId,
                        principalTable: "BudgetPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetPlanRestaurants_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 19, 23, 52, 15, 888, DateTimeKind.Utc).AddTicks(8335), "AQAAAAIAAYagAAAAEBg8OWE28f4whG1oDWVMrrBONPZhsIuj1Gpzs0jSpTExWed6z00L0ubZPXL369aa7Q==" });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlanAttractions_AttractionId",
                table: "BudgetPlanAttractions",
                column: "AttractionId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlanAttractions_BudgetPlanId",
                table: "BudgetPlanAttractions",
                column: "BudgetPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlanRestaurants_BudgetPlanId",
                table: "BudgetPlanRestaurants",
                column: "BudgetPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlanRestaurants_RestaurantId",
                table: "BudgetPlanRestaurants",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlans_HotelId",
                table: "BudgetPlans",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlans_UserId",
                table: "BudgetPlans",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetPlanAttractions");

            migrationBuilder.DropTable(
                name: "BudgetPlanRestaurants");

            migrationBuilder.DropTable(
                name: "BudgetPlans");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 18, 0, 0, 44, 937, DateTimeKind.Utc).AddTicks(1212), "AQAAAAIAAYagAAAAEAqD4hZ151FW3lPRYl1rQ/UMWyu+dd2tiy5GvoDoMkuEZvyyj9x0oJMaoiV+NYLEhA==" });
        }
    }
}
