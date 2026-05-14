using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voyagoo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeInscriptionToYear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "YearOfInscription",
                table: "Attractions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(
                "UPDATE [Attractions] SET [YearOfInscription] = YEAR([DateOfInscription])");

            migrationBuilder.DropColumn(
                name: "DateOfInscription",
                table: "Attractions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfInscription",
                table: "Attractions",
                nullable: false,
                defaultValue: new DateOnly(1900, 1, 1));

            migrationBuilder.Sql(
                "UPDATE [Attractions] SET [DateOfInscription] = DATEFROMPARTS([YearOfInscription], 1, 1)");

            migrationBuilder.DropColumn(
                name: "YearOfInscription",
                table: "Attractions");
        }
    }
}
