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

            modelBuilder.Entity<FavoriteHall>().HasOne(m => m.customer).WithMany(am => am.FavoriteHalls).HasForeignKey(m => m.CustomerId);
            modelBuilder.Entity<FavoriteHall>().HasOne(m => m.SpaceHall).WithMany(am => am.FavoriteHalls).HasForeignKey(m => m.HallId);
            
            //! TODO : Many to Many relations
            /*modelBuilder.Entity<BookingHallService>().HasOne(m => m.BookingHall).WithMany(am => am.).HasForeignKey(m => m.CustomerId);
            modelBuilder.Entity<FavoriteHall>().HasOne(m => m.SpaceHall).WithMany(am => am.FavoriteHalls).HasForeignKey(m => m.HallId);
            */
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
        public DbSet<SpaceService> SpaceService { get; set; }
        public DbSet<SpaceWorkingDay> SpaceWorkingDay { get; set; }
        public DbSet<SystemAdmin> SystemAdmin { get; set; }
        public DbSet<Title> Title { get; set; }



    }
}
