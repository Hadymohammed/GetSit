using GetSit.Data.Base;
using GetSit.Data.enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace GetSit.Models
{
    public class Payment : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public PaymentStatus Status { get; set; }
        public float TotalCost { get; set; }
        public float PaidAmount { get; set; }
        [Required]
        public DateTime LastUpdate { get; set; }
        [Required]
        public List<PaymentDetail> Details { get; set; }
        [DefaultValue(false),AllowNull]
        public bool? isGuest { get; set; }
        [AllowNull , ForeignKey("BookingId")]
        public int? BookingId { get; set; }
        public Booking ? Booking { get; set; }

        [AllowNull , ForeignKey("GuestBookingId")]
        public int? GuestBookingId { get; set; }
        public GuestBooking? GuestBooking { get; set; }
    }
}