using GetSit.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Numerics;

namespace GetSit.Data
{
    public class AppDBcontext : DbContext
    {
        public AppDBcontext(DbContextOptions<AppDBcontext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(TimeSpan))
                    {
                        property.SetColumnType("time(7)");
                    }
                }
            }

            //Customer Title and faculty
            modelBuilder.Entity<Customer>().HasOne(m => m.Faculty).WithMany(am => am.Customers).HasForeignKey(m => m.FacultyId);
            modelBuilder.Entity<Customer>().HasOne(m => m.Title).WithMany(am => am.Customers).HasForeignKey(m => m.TitleId);
            modelBuilder.Entity<PaymentCard>().HasOne(m => m.Customer).WithMany(am => am.PaymentCards).HasForeignKey(m => m.CustomerId);

            //FavoriteHall
            modelBuilder.Entity<FavoriteHall>().HasOne(m => m.customer).WithMany(am => am.FavoriteHalls).HasForeignKey(m => m.CustomerId);
            modelBuilder.Entity<FavoriteHall>().HasOne(m => m.SpaceHall).WithMany(am => am.FavoriteHalls).HasForeignKey(m => m.HallId);

            //Space Has Halls 
            modelBuilder.Entity<SpaceHall>().HasOne(m => m.Space).WithMany(am => am.Halls).HasForeignKey(m => m.SpaceId);

            //Space has services
            modelBuilder.Entity<SpaceService>().HasOne(m => m.Space).WithMany(am => am.Services).HasForeignKey(m => m.SpaceId);

            //Space has Photos and phones
            modelBuilder.Entity<SpacePhone>().HasOne(m => m.Space).WithMany(am => am.Phones).HasForeignKey(m => m.SpaceId);
            modelBuilder.Entity<SpacePhoto>().HasOne(m => m.Space).WithMany(am => am.Photos).HasForeignKey(m => m.SpaceId);
            modelBuilder.Entity<SpaceContact>().HasOne(m => m.Space).WithMany(am => am.SpaceContacts).HasForeignKey(m => m.SpaceId);

            //Hall Has Facilities and Photos
            modelBuilder.Entity<HallFacility>().HasOne(m => m.Hall).WithMany(am => am.HallFacilities).HasForeignKey(m => m.HallId);
            modelBuilder.Entity<HallPhoto>().HasOne(m => m.Hall).WithMany(am => am.HallPhotos).HasForeignKey(m => m.HallId);

            //Service has photos
            modelBuilder.Entity<ServicePhoto>().HasOne(m => m.SpaceService).WithMany(am => am.ServicePhotos).HasForeignKey(m => m.ServiceId);

            //Customer Booking
            modelBuilder.Entity<Booking>().HasOne(m => m.Customer).WithMany(am => am.Bookings).HasForeignKey(m => m.CustomerId);

            //Booking has halls
            modelBuilder.Entity<BookingHall>().HasOne(m => m.Booking).WithMany(am => am.BookingHalls).HasForeignKey(m => m.BookingId);
            modelBuilder.Entity<BookingHall>().HasOne(m => m.Hall).WithMany(am => am.Bookings).HasForeignKey(m => m.HallId);

            //Booking hall has service
            modelBuilder.Entity<BookingHallService>().HasOne(m => m.BookingHall).WithMany(am => am.BookedServices).HasForeignKey(m => m.BookingHallId);
            modelBuilder.Entity<BookingHallService>().HasOne(m => m.Service).WithMany(am => am.Bookings).HasForeignKey(m => m.ServiceId).OnDelete(DeleteBehavior.Restrict);

            //Booking has payment
            modelBuilder.Entity<Payment>().HasOne(m => m.Booking).WithOne(am => am.Payment);

            //Payment has details
            modelBuilder.Entity<PaymentDetail>().HasOne(m => m.Payment).WithMany(am => am.Details).HasForeignKey(m => m.PaymentId);
            modelBuilder.Entity<PaymentDetail>().HasOne(m => m.BookingHall).WithOne(am => am.paymentDetail);
            modelBuilder.Entity<PaymentDetail>().HasOne(m => m.BookingHallService).WithOne(am => am.PaymentDetail);




            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<BookingHall> BookingHall { get; set; }
        public DbSet<BookingHallService> BookingHallService { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Faculty> Faculty { get; set; }
        public DbSet<FavoriteHall> FavoriteHall { get; set; }
        public DbSet<HallFacility> HallFacility { get; set; }
        public DbSet<HallPhoto> HallPhoto { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<PaymentCard> PaymentCard { get; set; }
        public DbSet<PaymentDetail> PaymentDetail { get; set; }
        public DbSet<ServicePhoto> ServicePhoto { get; set; }
        public DbSet<Space> Space { get; set; }
        public DbSet<SpaceEmployee> SpaceEmployee { get; set; }
        public DbSet<SpaceHall> SpaceHall { get; set; }
        public DbSet<SpacePhone> SpacePhone { get; set; }
        public DbSet<SpacePhoto> SpacePhoto { get; set; }
        public DbSet<SpaceContact> spaceContact { get; set; }
        public DbSet<SpaceService> SpaceService { get; set; }
        public DbSet<SpaceWorkingDay> SpaceWorkingDay { get; set; }
        public DbSet<SystemAdmin> SystemAdmin { get; set; }
        public DbSet<Title> Title { get; set; }



    }
}