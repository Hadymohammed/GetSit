using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GetSit.Migrations
{
    /// <inheritdoc />
    public partial class BookingIdAllowNull_isGuestColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingHall_Booking_BookingId",
                table: "BookingHall");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingHall_GuestBooking_GuestBookingId",
                table: "BookingHall");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Booking_BookingId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_GuestBooking_GuestBookingId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_BookingId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_GuestBookingId",
                table: "Payment");

            migrationBuilder.AlterColumn<string>(
                name: "BankAccount",
                table: "Space",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "GuestBookingId",
                table: "Payment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BookingId",
                table: "Payment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "isGuest",
                table: "Payment",
                type: "bit",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GuestBookingId",
                table: "BookingHall",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BookingId",
                table: "BookingHall",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "isGuest",
                table: "BookingHall",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_BookingId",
                table: "Payment",
                column: "BookingId",
                unique: true,
                filter: "[BookingId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_GuestBookingId",
                table: "Payment",
                column: "GuestBookingId",
                unique: true,
                filter: "[GuestBookingId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingHall_Booking_BookingId",
                table: "BookingHall",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingHall_GuestBooking_GuestBookingId",
                table: "BookingHall",
                column: "GuestBookingId",
                principalTable: "GuestBooking",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Booking_BookingId",
                table: "Payment",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_GuestBooking_GuestBookingId",
                table: "Payment",
                column: "GuestBookingId",
                principalTable: "GuestBooking",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingHall_Booking_BookingId",
                table: "BookingHall");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingHall_GuestBooking_GuestBookingId",
                table: "BookingHall");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Booking_BookingId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_GuestBooking_GuestBookingId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_BookingId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_GuestBookingId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "isGuest",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "isGuest",
                table: "BookingHall");

            migrationBuilder.AlterColumn<string>(
                name: "BankAccount",
                table: "Space",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GuestBookingId",
                table: "Payment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BookingId",
                table: "Payment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GuestBookingId",
                table: "BookingHall",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BookingId",
                table: "BookingHall",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_BookingId",
                table: "Payment",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_GuestBookingId",
                table: "Payment",
                column: "GuestBookingId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingHall_Booking_BookingId",
                table: "BookingHall",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingHall_GuestBooking_GuestBookingId",
                table: "BookingHall",
                column: "GuestBookingId",
                principalTable: "GuestBooking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Booking_BookingId",
                table: "Payment",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_GuestBooking_GuestBookingId",
                table: "Payment",
                column: "GuestBookingId",
                principalTable: "GuestBooking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
