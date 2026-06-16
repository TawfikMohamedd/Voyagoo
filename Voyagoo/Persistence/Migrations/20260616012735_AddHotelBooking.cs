using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voyagoo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddHotelBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HotelBookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CheckIn = table.Column<DateOnly>(type: "date", nullable: false),
                    CheckOut = table.Column<DateOnly>(type: "date", nullable: false),
                    Nights = table.Column<int>(type: "int", nullable: false),
                    RoomsTotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    BoardsTotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ExtrasTotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ServiceChargePercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ServiceChargeAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelBookings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HotelBookings_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HotelBookingFeatureSelections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelBookingId = table.Column<int>(type: "int", nullable: false),
                    BookingFeatureId = table.Column<int>(type: "int", nullable: false),
                    RoomsCount = table.Column<int>(type: "int", nullable: false),
                    PricePerNight = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelBookingFeatureSelections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelBookingFeatureSelections_BookingFeatures_BookingFeatureId",
                        column: x => x.BookingFeatureId,
                        principalTable: "BookingFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HotelBookingFeatureSelections_HotelBookings_HotelBookingId",
                        column: x => x.HotelBookingId,
                        principalTable: "HotelBookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HotelBookingRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelBookingId = table.Column<int>(type: "int", nullable: false),
                    RoomType = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    PricePerNight = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelBookingRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelBookingRooms_HotelBookings_HotelBookingId",
                        column: x => x.HotelBookingId,
                        principalTable: "HotelBookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 16, 1, 27, 34, 31, DateTimeKind.Utc).AddTicks(5361), "AQAAAAIAAYagAAAAEExTdliW0yhr1sqWqDJX68xJ5gBO1eYde6MP5sxIYn2dUBboT9Qs+n4Zu89If9bFjg==" });

            migrationBuilder.CreateIndex(
                name: "IX_HotelBookingFeatureSelections_BookingFeatureId",
                table: "HotelBookingFeatureSelections",
                column: "BookingFeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelBookingFeatureSelections_HotelBookingId",
                table: "HotelBookingFeatureSelections",
                column: "HotelBookingId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelBookingRooms_HotelBookingId",
                table: "HotelBookingRooms",
                column: "HotelBookingId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelBookings_HotelId",
                table: "HotelBookings",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelBookings_UserId",
                table: "HotelBookings",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HotelBookingFeatureSelections");

            migrationBuilder.DropTable(
                name: "HotelBookingRooms");

            migrationBuilder.DropTable(
                name: "HotelBookings");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 15, 6, 20, 45, 420, DateTimeKind.Utc).AddTicks(8216), "AQAAAAIAAYagAAAAECJSluIRJEdMAJU4L5FUXsoGPNNNBsgZEi0XX6m7Y+Cuhm8mz9c2UCIwqPHNvnwgpQ==" });
        }
    }
}
