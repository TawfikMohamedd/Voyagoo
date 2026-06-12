using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voyagoo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DeleteHotelsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Hotels_HotelId",
                table: "Favorites");

            migrationBuilder.DropTable(
                name: "BookingFeatures");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "HotelDisplayFeatures");

            migrationBuilder.DropTable(
                name: "HotelFeatures");

            migrationBuilder.DropTable(
                name: "HotelImages");

            migrationBuilder.DropTable(
                name: "HotelBookings");

            migrationBuilder.DropTable(
                name: "DisplayFeatures");

            migrationBuilder.DropTable(
                name: "HotelFeatureEntities");

            migrationBuilder.DropTable(
                name: "Hotels");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_HotelId",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_UserId_HotelId",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "Favorites");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHJMzUQQyCudpyHo7WaMHKch5vLv8/11oB/EoagZwTWq2kuWgUcn2A6YQHC2aK6EwA==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Favorites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "HotelId",
                table: "Favorites",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DisplayFeatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisplayFeatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HotelFeatureEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelFeatureEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hotels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoublePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    DoubleRooms = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PricePerNight = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(3,1)", nullable: false),
                    StandardPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    StandardRooms = table.Column<int>(type: "int", nullable: false),
                    SuitePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SuiteRooms = table.Column<int>(type: "int", nullable: false),
                    TriplePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TripleRooms = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hotels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HotelBookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelId = table.Column<int>(type: "int", nullable: false),
                    DoubleRooms = table.Column<int>(type: "int", nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RemainingAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ServiceFee = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    StandardRooms = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SuiteRooms = table.Column<int>(type: "int", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TripleRooms = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelBookings_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HotelDisplayFeatures",
                columns: table => new
                {
                    HotelId = table.Column<int>(type: "int", nullable: false),
                    DisplayFeatureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelDisplayFeatures", x => new { x.HotelId, x.DisplayFeatureId });
                    table.ForeignKey(
                        name: "FK_HotelDisplayFeatures_DisplayFeatures_DisplayFeatureId",
                        column: x => x.DisplayFeatureId,
                        principalTable: "DisplayFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HotelDisplayFeatures_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HotelFeatures",
                columns: table => new
                {
                    HotelId = table.Column<int>(type: "int", nullable: false),
                    FeatureId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelFeatures", x => new { x.HotelId, x.FeatureId });
                    table.ForeignKey(
                        name: "FK_HotelFeatures_HotelFeatureEntities_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "HotelFeatureEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HotelFeatures_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HotelImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelImages_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingFeatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelBookingId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingFeatures_HotelBookings_HotelBookingId",
                        column: x => x.HotelBookingId,
                        principalTable: "HotelBookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKFydCC4ngYUG/7UvsiyfISRWjSzxQFhLJIFWcg1Gm8SD5/g/rXxTPofwkUU6C3ENA==");

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

            migrationBuilder.CreateIndex(
                name: "IX_BookingFeatures_HotelBookingId",
                table: "BookingFeatures",
                column: "HotelBookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_HotelId",
                table: "Comments",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelBookings_HotelId",
                table: "HotelBookings",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelDisplayFeatures_DisplayFeatureId",
                table: "HotelDisplayFeatures",
                column: "DisplayFeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelFeatures_FeatureId",
                table: "HotelFeatures",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelImages_HotelId",
                table: "HotelImages",
                column: "HotelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Hotels_HotelId",
                table: "Favorites",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id");
        }
    }
}
