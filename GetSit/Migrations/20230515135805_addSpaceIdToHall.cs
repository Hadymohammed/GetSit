using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GetSit.Migrations
{
    /// <inheritdoc />
    public partial class addSpaceIdToHall : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpaceHall_Space_SpaceId",
                table: "SpaceHall");

            migrationBuilder.AlterColumn<int>(
                name: "SpaceId",
                table: "SpaceHall",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SpaceHall_Space_SpaceId",
                table: "SpaceHall",
                column: "SpaceId",
                principalTable: "Space",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpaceHall_Space_SpaceId",
                table: "SpaceHall");

            migrationBuilder.AlterColumn<int>(
                name: "SpaceId",
                table: "SpaceHall",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_SpaceHall_Space_SpaceId",
                table: "SpaceHall",
                column: "SpaceId",
                principalTable: "Space",
                principalColumn: "Id");
        }
    }
}
