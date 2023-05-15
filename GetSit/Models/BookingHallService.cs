using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GetSit.Models
{
    public class BookingHallService
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int NumberOfUnits { get; set; }
        [Required]
        public float PricePerUnit { get; set; }
        [Required,ForeignKey("BookingHallId")]
        public int BookingHallId { get; set; }
        public BookingHall BookingHall { get; set; }
        public SpaceService Service { get; set; }
        [Required,ForeignKey("ServiceId")]
        public int ServiceId { get; set; }
        public PaymentDetail PaymentDetail { get; set; }
    }
}
