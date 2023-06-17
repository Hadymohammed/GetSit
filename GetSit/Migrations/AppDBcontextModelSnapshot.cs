// <auto-generated />
using System;
using GetSit.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GetSit.Migrations
{
    [DbContext(typeof(AppDBcontext))]
    partial class AppDBcontextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GetSit.Models.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("BookingDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("BookingStatus")
                        .HasColumnType("int");

                    b.Property<int>("BookingType")
                        .HasColumnType("int");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DesiredDate")
                        .HasColumnType("datetime2");

                    b.Property<float>("NumberOfHours")
                        .HasColumnType("real");

                    b.Property<float>("Paid")
                        .HasColumnType("real");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time(7)");

                    b.Property<float>("TotalCost")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Booking");
                });

            modelBuilder.Entity("GetSit.Models.BookingHall", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("BookingId")
                        .HasColumnType("int");

                    b.Property<int?>("GuestBookingId")
                        .HasColumnType("int");

                    b.Property<int>("HallId")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfUnits")
                        .HasColumnType("int");

                    b.Property<float>("PricePerUnit")
                        .HasColumnType("real");

                    b.Property<bool?>("isGuest")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("BookingId");

                    b.HasIndex("GuestBookingId");

                    b.HasIndex("HallId");

                    b.ToTable("BookingHall");
                });

            modelBuilder.Entity("GetSit.Models.BookingHallService", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BookingHallId")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfUnits")
                        .HasColumnType("int");

                    b.Property<float>("PricePerUnit")
                        .HasColumnType("real");

                    b.Property<int>("ServiceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BookingHallId");

                    b.HasIndex("ServiceId");

                    b.ToTable("BookingHallService");
                });

            modelBuilder.Entity("GetSit.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Birthdate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Blocked")
                        .HasColumnType("bit");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CoverPrictureUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CustomerType")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FacultyId")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Penality")
                        .HasColumnType("int");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePictureUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TitleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FacultyId");

                    b.HasIndex("TitleId");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("GetSit.Models.Faculty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Faculty");
                });

            modelBuilder.Entity("GetSit.Models.FavoriteHall", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<int>("HallId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("HallId");

                    b.ToTable("FavoriteHall");
                });

            modelBuilder.Entity("GetSit.Models.GuestBooking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("BookingDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("BookingStatus")
                        .HasColumnType("int");

                    b.Property<DateTime>("DesiredDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("EndTime")
                        .HasColumnType("time(7)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Paid")
                        .HasColumnType("real");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time(7)");

                    b.Property<float>("TotalCost")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("GuestBooking");
                });

            modelBuilder.Entity("GetSit.Models.HallFacility", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Facility")
                        .HasColumnType("int");

                    b.Property<int>("HallId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("HallId");

                    b.ToTable("HallFacility");
                });

            modelBuilder.Entity("GetSit.Models.HallPhoto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("HallId")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("HallId");

                    b.ToTable("HallPhoto");
                });

            modelBuilder.Entity("GetSit.Models.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("BookingId")
                        .HasColumnType("int");

                    b.Property<int?>("GuestBookingId")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<float>("PaidAmount")
                        .HasColumnType("real");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<float>("TotalCost")
                        .HasColumnType("real");

                    b.Property<bool?>("isGuest")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("BookingId")
                        .IsUnique()
                        .HasFilter("[BookingId] IS NOT NULL");
                    b.HasIndex("GuestBookingId")
                        .IsUnique()
                        .HasFilter("[GuestBookingId] IS NOT NULL");
                    b.ToTable("Payment");
                });

            modelBuilder.Entity("GetSit.Models.PaymentCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CCV")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ExpireDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("PaymentCard");
                });

            modelBuilder.Entity("GetSit.Models.PaymentDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("BookingHallId")
                        .HasColumnType("int");

                    b.Property<int?>("BookingHallServiceId")
                        .HasColumnType("int");

                    b.Property<int>("PaymentId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<float>("TotalCost")
                        .HasColumnType("real");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BookingHallId")
                        .IsUnique()
                        .HasFilter("[BookingHallId] IS NOT NULL");

                    b.HasIndex("BookingHallServiceId")
                        .IsUnique()
                        .HasFilter("[BookingHallServiceId] IS NOT NULL");

                    b.HasIndex("PaymentId");

                    b.ToTable("PaymentDetail");
                });

            modelBuilder.Entity("GetSit.Models.ServicePhoto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ServiceId")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ServiceId");

                    b.ToTable("ServicePhoto");
                });

            modelBuilder.Entity("GetSit.Models.Space", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("BankAccount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Bio")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Facebook")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GPSLocation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Instagram")
                        .HasColumnType("nvarchar(max)");
                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<bool>("IsFast")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SpaceCover")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SpaceLogo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Twitter")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Space");
                });

            modelBuilder.Entity("GetSit.Models.SpaceEmployee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Birthdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CoverPrictureUrl")

                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EmployeeRole")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePictureUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SpaceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SpaceId");

                    b.ToTable("SpaceEmployee");
                });

            modelBuilder.Entity("GetSit.Models.SpaceHall", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<float>("CostPerHour")
                        .HasColumnType("real");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SpaceId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Thumbnail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SpaceId");

                    b.ToTable("SpaceHall");
                });

            modelBuilder.Entity("GetSit.Models.SpacePhone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SpaceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SpaceId");

                    b.ToTable("SpacePhone");
                });

            modelBuilder.Entity("GetSit.Models.SpacePhoto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("SpaceId")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SpaceId");

                    b.ToTable("SpacePhoto");
                });

            modelBuilder.Entity("GetSit.Models.SpaceService", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<int>("SpaceId")
                        .HasColumnType("int");

                    b.Property<string>("Thumbnail")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SpaceId");

                    b.ToTable("SpaceService");
                });

            modelBuilder.Entity("GetSit.Models.SpaceWorkingDay", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<TimeSpan>("ClosingTime")
                        .HasColumnType("time(7)");

                    b.Property<int>("Day")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("OpeningTime")
                        .HasColumnType("time(7)");

                    b.Property<int>("SpaceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SpaceId");

                    b.ToTable("SpaceWorkingDay");
                });

            modelBuilder.Entity("GetSit.Models.SystemAdmin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AdminRole")
                        .HasColumnType("int");

                    b.Property<DateTime>("Birthdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CoverPrictureUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePictureUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SystemAdmin");
                });

            modelBuilder.Entity("GetSit.Models.Title", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Title");
                });

            modelBuilder.Entity("GetSit.Models.Booking", b =>
                {
                    b.HasOne("GetSit.Models.Customer", "Customer")
                        .WithMany("Bookings")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("GetSit.Models.BookingHall", b =>
                {
                    b.HasOne("GetSit.Models.Booking", "Booking")
                        .WithMany("BookingHalls")
                        .HasForeignKey("BookingId");

                    b.HasOne("GetSit.Models.GuestBooking", "GuestBooking")
                        .WithMany("BookingHalls")
                        .HasForeignKey("GuestBookingId");

                    b.HasOne("GetSit.Models.SpaceHall", "Hall")
                        .WithMany("Bookings")
                        .HasForeignKey("HallId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");

                    b.Navigation("GuestBooking");

                    b.Navigation("Hall");
                });

            modelBuilder.Entity("GetSit.Models.BookingHallService", b =>
                {
                    b.HasOne("GetSit.Models.BookingHall", "BookingHall")
                        .WithMany("BookedServices")
                        .HasForeignKey("BookingHallId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GetSit.Models.SpaceService", "Service")
                        .WithMany("Bookings")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("BookingHall");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("GetSit.Models.Customer", b =>
                {
                    b.HasOne("GetSit.Models.Faculty", "Faculty")
                        .WithMany("Customers")
                        .HasForeignKey("FacultyId");

                    b.HasOne("GetSit.Models.Title", "Title")
                        .WithMany("Customers")
                        .HasForeignKey("TitleId");

                    b.Navigation("Faculty");

                    b.Navigation("Title");
                });

            modelBuilder.Entity("GetSit.Models.FavoriteHall", b =>
                {
                    b.HasOne("GetSit.Models.Customer", "customer")
                        .WithMany("FavoriteHalls")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GetSit.Models.SpaceHall", "SpaceHall")
                        .WithMany("FavoriteHalls")
                        .HasForeignKey("HallId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SpaceHall");

                    b.Navigation("customer");
                });

            modelBuilder.Entity("GetSit.Models.GuestBooking", b =>
                {
                    b.HasOne("GetSit.Models.SpaceEmployee", "Employee")
                        .WithMany("GuestBookings")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("GetSit.Models.HallFacility", b =>
                {
                    b.HasOne("GetSit.Models.SpaceHall", "Hall")
                        .WithMany("HallFacilities")
                        .HasForeignKey("HallId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hall");
                });

            modelBuilder.Entity("GetSit.Models.HallPhoto", b =>
                {
                    b.HasOne("GetSit.Models.SpaceHall", "Hall")
                        .WithMany("HallPhotos")
                        .HasForeignKey("HallId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hall");
                });

            modelBuilder.Entity("GetSit.Models.Payment", b =>
                {
                    b.HasOne("GetSit.Models.Booking", "Booking")
                        .WithOne("Payment")
                        .HasForeignKey("GetSit.Models.Payment", "BookingId");

                    b.HasOne("GetSit.Models.GuestBooking", "GuestBooking")
                        .WithOne("Payment")
                        .HasForeignKey("GetSit.Models.Payment", "GuestBookingId");

                    b.Navigation("Booking");

                    b.Navigation("GuestBooking");
                });

            modelBuilder.Entity("GetSit.Models.PaymentCard", b =>
                {
                    b.HasOne("GetSit.Models.Customer", "Customer")
                        .WithMany("PaymentCards")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("GetSit.Models.PaymentDetail", b =>
                {
                    b.HasOne("GetSit.Models.BookingHall", "BookingHall")
                        .WithOne("paymentDetail")
                        .HasForeignKey("GetSit.Models.PaymentDetail", "BookingHallId");

                    b.HasOne("GetSit.Models.BookingHallService", "BookingHallService")
                        .WithOne("PaymentDetail")
                        .HasForeignKey("GetSit.Models.PaymentDetail", "BookingHallServiceId");

                    b.HasOne("GetSit.Models.Payment", "Payment")
                        .WithMany("Details")
                        .HasForeignKey("PaymentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BookingHall");

                    b.Navigation("BookingHallService");

                    b.Navigation("Payment");
                });

            modelBuilder.Entity("GetSit.Models.ServicePhoto", b =>
                {
                    b.HasOne("GetSit.Models.SpaceService", "SpaceService")
                        .WithMany("ServicePhotos")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SpaceService");
                });

            modelBuilder.Entity("GetSit.Models.SpaceEmployee", b =>
                {
                    b.HasOne("GetSit.Models.Space", "Space")
                        .WithMany("Employees")
                        .HasForeignKey("SpaceId");

                    b.Navigation("Space");
                });

            modelBuilder.Entity("GetSit.Models.SpaceHall", b =>
                {
                    b.HasOne("GetSit.Models.Space", "Space")
                        .WithMany("Halls")
                        .HasForeignKey("SpaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Space");
                });

            modelBuilder.Entity("GetSit.Models.SpacePhone", b =>
                {
                    b.HasOne("GetSit.Models.Space", "Space")
                        .WithMany("Phones")
                        .HasForeignKey("SpaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Space");
                });

            modelBuilder.Entity("GetSit.Models.SpacePhoto", b =>
                {
                    b.HasOne("GetSit.Models.Space", "Space")
                        .WithMany("Photos")
                        .HasForeignKey("SpaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Space");
                });

            modelBuilder.Entity("GetSit.Models.SpaceService", b =>
                {
                    b.HasOne("GetSit.Models.Space", "Space")
                        .WithMany("Services")
                        .HasForeignKey("SpaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Space");
                });

            modelBuilder.Entity("GetSit.Models.SpaceWorkingDay", b =>
                {
                    b.HasOne("GetSit.Models.Space", "Space")
                        .WithMany("WorkingDays")
                        .HasForeignKey("SpaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Space");
                });

            modelBuilder.Entity("GetSit.Models.Booking", b =>
                {
                    b.Navigation("BookingHalls");

                    b.Navigation("Payment")
                        .IsRequired();
                });

            modelBuilder.Entity("GetSit.Models.BookingHall", b =>
                {
                    b.Navigation("BookedServices");

                    b.Navigation("paymentDetail")
                        .IsRequired();
                });

            modelBuilder.Entity("GetSit.Models.BookingHallService", b =>
                {
                    b.Navigation("PaymentDetail")
                        .IsRequired();
                });

            modelBuilder.Entity("GetSit.Models.Customer", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("FavoriteHalls");

                    b.Navigation("PaymentCards");
                });

            modelBuilder.Entity("GetSit.Models.Faculty", b =>
                {
                    b.Navigation("Customers");
                });

            modelBuilder.Entity("GetSit.Models.GuestBooking", b =>
                {
                    b.Navigation("BookingHalls");

                    b.Navigation("Payment")
                        .IsRequired();
                });

            modelBuilder.Entity("GetSit.Models.Payment", b =>
                {
                    b.Navigation("Details");
                });

            modelBuilder.Entity("GetSit.Models.Space", b =>
                {
                    b.Navigation("Employees");

                    b.Navigation("Halls");

                    b.Navigation("Phones");

                    b.Navigation("Photos");

                    b.Navigation("Services");

                    b.Navigation("WorkingDays");
                });

            modelBuilder.Entity("GetSit.Models.SpaceEmployee", b =>
                {
                    b.Navigation("GuestBookings");
                });

            modelBuilder.Entity("GetSit.Models.SpaceHall", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("FavoriteHalls");

                    b.Navigation("HallFacilities");

                    b.Navigation("HallPhotos");
                });

            modelBuilder.Entity("GetSit.Models.SpaceService", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("ServicePhotos");
                });

            modelBuilder.Entity("GetSit.Models.Title", b =>
                {
                    b.Navigation("Customers");
                });
#pragma warning restore 612, 618
        }
    }
}
