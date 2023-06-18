using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GetSit.Migrations
{
    /// <inheritdoc />
    public partial class AddTokenTable_IsRegisteredColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Registerd",
                table: "SystemAdmin",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Registerd",
                table: "SpaceEmployee",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Token",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Token", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Token");

            migrationBuilder.DropColumn(
                name: "Registerd",
                table: "SystemAdmin");

            migrationBuilder.DropColumn(
                name: "Registerd",
                table: "SpaceEmployee");
        }
    }
}
