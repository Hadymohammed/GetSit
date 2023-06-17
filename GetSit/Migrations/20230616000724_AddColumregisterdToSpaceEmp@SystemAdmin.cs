using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GetSit.Migrations
{
    /// <inheritdoc />
    public partial class AddColumregisterdToSpaceEmpSystemAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpaceEmployee_Space_SpaceId",
                table: "SpaceEmployee");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "SystemAdmin",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "SystemAdmin",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "Registerd",
                table: "SystemAdmin",
                type: "bit",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SpaceId",
                table: "SpaceEmployee",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "SpaceEmployee",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "SpaceEmployee",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

            migrationBuilder.AddForeignKey(
                name: "FK_SpaceEmployee_Space_SpaceId",
                table: "SpaceEmployee",
                column: "SpaceId",
                principalTable: "Space",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpaceEmployee_Space_SpaceId",
                table: "SpaceEmployee");

            migrationBuilder.DropTable(
                name: "Token");

            migrationBuilder.DropColumn(
                name: "Registerd",
                table: "SystemAdmin");

            migrationBuilder.DropColumn(
                name: "Registerd",
                table: "SpaceEmployee");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "SystemAdmin",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "SystemAdmin",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SpaceId",
                table: "SpaceEmployee",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "SpaceEmployee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "SpaceEmployee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SpaceEmployee_Space_SpaceId",
                table: "SpaceEmployee",
                column: "SpaceId",
                principalTable: "Space",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
