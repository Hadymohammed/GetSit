using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GetSit.Migrations
{
    /// <inheritdoc />
    public partial class SpaceWorkingDayEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "SpaceWorkingDay");

            migrationBuilder.AddColumn<int>(
                name: "Day",
                table: "SpaceWorkingDay",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "SpaceWorkingDay");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "SpaceWorkingDay",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
