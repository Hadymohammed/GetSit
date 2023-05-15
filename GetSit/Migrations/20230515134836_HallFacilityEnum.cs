using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GetSit.Migrations
{
    /// <inheritdoc />
    public partial class HallFacilityEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "HallFacility");

            migrationBuilder.AddColumn<int>(
                name: "Facility",
                table: "HallFacility",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Facility",
                table: "HallFacility");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "HallFacility",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
