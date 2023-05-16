using GetSit.Data.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GetSit.Models
{
    public class Payment
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
        [ForeignKey("BookingId")]
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
    }
}
