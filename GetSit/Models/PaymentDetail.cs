using GetSit.Data.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace GetSit.Models
{
    public class PaymentDetail
    {
        [Key]
        public int Id { get; set; }
        public float TotalCost { get; set; }
        [Required]
        public PaymentStatus Status { get; set; }
        [Required]
        public  PaymentType Type { get; set; }
        [ForeignKey("PaymentId")]
        public int PaymentId { get; set; }
        public Payment? Payment { get; set; }
        [ForeignKey("BookingHallId"),AllowNull]
        public int? BookingHallId { get; set; }
        public  BookingHall? BookingHall { get; set; }
        [ForeignKey("BookingHallServiceId"), AllowNull]
        public int? BookingHallServiceId { get; set; }
        public BookingHallService? BookingHallService { get; set; }
        
    }
}
