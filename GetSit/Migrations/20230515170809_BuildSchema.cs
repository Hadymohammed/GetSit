using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GetSit.Migrations
{
    /// <inheritdoc />
    public partial class BuildSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Faculty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculty", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Space",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GPSLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsFast = table.Column<bool>(type: "bit", nullable: false),
                    BankAccount = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Space", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemAdmin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminRole = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemAdmin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Title",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Title", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpaceEmployee",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeRole = table.Column<int>(type: "int", nullable: false),
                    SpaceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpaceEmployee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpaceEmployee_Space_SpaceId",
                        column: x => x.SpaceId,
                        principalTable: "Space",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpaceHall",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpaceId = table.Column<int>(type: "int", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    CostPerHour = table.Column<float>(type: "real", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpaceHall", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpaceHall_Space_SpaceId",
                        column: x => x.SpaceId,
                        principalTable: "Space",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpacePhone",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpaceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpacePhone", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpacePhone_Space_SpaceId",
                        column: x => x.SpaceId,
                        principalTable: "Space",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpacePhoto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpaceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpacePhoto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpacePhoto_Space_SpaceId",
                        column: x => x.SpaceId,
                        principalTable: "Space",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpaceService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpaceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpaceService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpaceService_Space_SpaceId",
                        column: x => x.SpaceId,
                        principalTable: "Space",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpaceWorkingDay",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(type: "int", nullable: false),
                    OpeningTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClosingTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SpaceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpaceWorkingDay", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpaceWorkingDay_Space_SpaceId",
                        column: x => x.SpaceId,
                        principalTable: "Space",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerType = table.Column<int>(type: "int", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Penality = table.Column<int>(type: "int", nullable: false),
                    Blocked = table.Column<bool>(type: "bit", nullable: false),
                    FacultyId = table.Column<int>(type: "int", nullable: true),
                    TitleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_Faculty_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculty",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customer_Title_TitleId",
                        column: x => x.TitleId,
                        principalTable: "Title",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HallFacility",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Facility = table.Column<int>(type: "int", nullable: false),
                    HallId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HallFacility", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HallFacility_SpaceHall_HallId",
                        column: x => x.HallId,
                        principalTable: "SpaceHall",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HallPhoto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HallId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HallPhoto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HallPhoto_SpaceHall_HallId",
                        column: x => x.HallId,
                        principalTable: "SpaceHall",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServicePhoto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePhoto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServicePhoto_SpaceService_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "SpaceService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DesiredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumberOfHours = table.Column<float>(type: "real", nullable: false),
                    TotalCost = table.Column<float>(type: "real", nullable: false),
                    Paid = table.Column<float>(type: "real", nullable: false),
                    BookingStatus = table.Column<int>(type: "int", nullable: false),
                    BookingType = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Booking_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteHall",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    HallId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteHall", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteHall_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteHall_SpaceHall_HallId",
                        column: x => x.HallId,
                        principalTable: "SpaceHall",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentCard",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CCV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentCard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentCard_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingHall",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumberOfUnits = table.Column<int>(type: "int", nullable: false),
                    PricePerUnit = table.Column<float>(type: "real", nullable: false),
                    HallId = table.Column<int>(type: "int", nullable: false),
                    BookingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingHall", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingHall_Booking_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingHall_SpaceHall_HallId",
                        column: x => x.HallId,
                        principalTable: "SpaceHall",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalCost = table.Column<float>(type: "real", nullable: false),
                    PaidAmount = table.Column<float>(type: "real", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BookingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payment_Booking_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingHallService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumberOfUnits = table.Column<int>(type: "int", nullable: false),
                    PricePerUnit = table.Column<float>(type: "real", nullable: false),
                    BookingHallId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingHallService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingHallService_BookingHall_BookingHallId",
                        column: x => x.BookingHallId,
                        principalTable: "BookingHall",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingHallService_SpaceService_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "SpaceService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalCost = table.Column<float>(type: "real", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    BookingHallId = table.Column<int>(type: "int", nullable: true),
                    BookingHallServiceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentDetail_BookingHallService_BookingHallServiceId",
                        column: x => x.BookingHallServiceId,
                        principalTable: "BookingHallService",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PaymentDetail_BookingHall_BookingHallId",
                        column: x => x.BookingHallId,
                        principalTable: "BookingHall",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PaymentDetail_Payment_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_CustomerId",
                table: "Booking",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingHall_BookingId",
                table: "BookingHall",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingHall_HallId",
                table: "BookingHall",
                column: "HallId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingHallService_BookingHallId",
                table: "BookingHallService",
                column: "BookingHallId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingHallService_ServiceId",
                table: "BookingHallService",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_FacultyId",
                table: "Customer",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_TitleId",
                table: "Customer",
                column: "TitleId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteHall_CustomerId",
                table: "FavoriteHall",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteHall_HallId",
                table: "FavoriteHall",
                column: "HallId");

            migrationBuilder.CreateIndex(
                name: "IX_HallFacility_HallId",
                table: "HallFacility",
                column: "HallId");

            migrationBuilder.CreateIndex(
                name: "IX_HallPhoto_HallId",
                table: "HallPhoto",
                column: "HallId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_BookingId",
                table: "Payment",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCard_CustomerId",
                table: "PaymentCard",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDetail_BookingHallId",
                table: "PaymentDetail",
                column: "BookingHallId",
                unique: true,
                filter: "[BookingHallId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDetail_BookingHallServiceId",
                table: "PaymentDetail",
                column: "BookingHallServiceId",
                unique: true,
                filter: "[BookingHallServiceId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDetail_PaymentId",
                table: "PaymentDetail",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePhoto_ServiceId",
                table: "ServicePhoto",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_SpaceEmployee_SpaceId",
                table: "SpaceEmployee",
                column: "SpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_SpaceHall_SpaceId",
                table: "SpaceHall",
                column: "SpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_SpacePhone_SpaceId",
                table: "SpacePhone",
                column: "SpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_SpacePhoto_SpaceId",
                table: "SpacePhoto",
                column: "SpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_SpaceService_SpaceId",
                table: "SpaceService",
                column: "SpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_SpaceWorkingDay_SpaceId",
                table: "SpaceWorkingDay",
                column: "SpaceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteHall");

            migrationBuilder.DropTable(
                name: "HallFacility");

            migrationBuilder.DropTable(
                name: "HallPhoto");

            migrationBuilder.DropTable(
                name: "PaymentCard");

            migrationBuilder.DropTable(
                name: "PaymentDetail");

            migrationBuilder.DropTable(
                name: "ServicePhoto");

            migrationBuilder.DropTable(
                name: "SpaceEmployee");

            migrationBuilder.DropTable(
                name: "SpacePhone");

            migrationBuilder.DropTable(
                name: "SpacePhoto");

            migrationBuilder.DropTable(
                name: "SpaceWorkingDay");

            migrationBuilder.DropTable(
                name: "SystemAdmin");

            migrationBuilder.DropTable(
                name: "BookingHallService");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "BookingHall");

            migrationBuilder.DropTable(
                name: "SpaceService");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "SpaceHall");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Space");

            migrationBuilder.DropTable(
                name: "Faculty");

            migrationBuilder.DropTable(
                name: "Title");
        }
    }
}
