using GetSit.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace GetSit.Models
{
    public class BookingHall:IEntityBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int NumberOfUnits { get; set; }
        [Required]
        public float PricePerUnit { get; set; }

        [Required, ForeignKey("HallId")]
        public int HallId { get; set; }
        [Required]
        public SpaceHall Hall { get; set; }
        [AllowNull, ForeignKey("BookingId")]
        public int BookingId { get; set; }
        public Booking? Booking { get; set; }

        [AllowNull, ForeignKey("GuestBookingId")]
        public int GuestBookingId { get; set; }
        public GuestBooking? GuestBooking { get; set; }

        public PaymentDetail paymentDetail { get; set; }
        public List<BookingHallService>? BookedServices { get; set; }
    }
}