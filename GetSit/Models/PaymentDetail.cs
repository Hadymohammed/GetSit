using GetSit.Data.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Required]
        public Payment Payment { get; set; }
        [ForeignKey("BookingHallId")]
        public int BookingHallId { get; set; }
        public  BookingHall BookingHall { get; set; }
        [ForeignKey("BookingHallServiceId")]
        public int BookingHallServiceId { get; set; }
        [Required]
        public BookingHallService BookingHallService { get; set; }
        
    }
}
