using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusTicketAI.API.Migrations
{
    /// <inheritdoc />
    public partial class AddBusDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BusType",
                table: "Buses",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Buses",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusType",
                table: "Buses");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Buses");
        }
    }
}
