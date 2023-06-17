using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GetSit.Migrations
{
    /// <inheritdoc />
    public partial class GuestBooking : Migration
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

            migrationBuilder.AddColumn<int>(
                name: "GuestBookingId",
                table: "Payment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GuestBookingId",
                table: "BookingHall",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GuestBooking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DesiredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time(7)", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time(7)", nullable: false),
                    TotalCost = table.Column<float>(type: "real", nullable: false),
                    Paid = table.Column<float>(type: "real", nullable: false),
                    BookingStatus = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuestBooking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuestBooking_SpaceEmployee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "SpaceEmployee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payment_GuestBookingId",
                table: "Payment",
                column: "GuestBookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingHall_GuestBookingId",
                table: "BookingHall",
                column: "GuestBookingId");

            migrationBuilder.CreateIndex(
                name: "IX_GuestBooking_EmployeeId",
                table: "GuestBooking",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingHall_GuestBooking_GuestBookingId",
                table: "BookingHall",
                column: "GuestBookingId",
                principalTable: "GuestBooking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_GuestBooking_GuestBookingId",
                table: "Payment",
                column: "GuestBookingId",
                principalTable: "GuestBooking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_BookingHall_GuestBooking_GuestBookingId",
                table: "BookingHall");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_GuestBooking_GuestBookingId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_SpaceEmployee_Space_SpaceId",
                table: "SpaceEmployee");

            migrationBuilder.DropTable(
                name: "GuestBooking");

            migrationBuilder.DropIndex(
                name: "IX_Payment_GuestBookingId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_BookingHall_GuestBookingId",
                table: "BookingHall");

            migrationBuilder.DropColumn(
                name: "GuestBookingId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "GuestBookingId",
                table: "BookingHall");

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
