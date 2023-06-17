using GetSit.Data.Base;
using GetSit.Data.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GetSit.Models
{
    public class GuestBooking : IEntityBase
    {
        // guest info
        [Key]
        public int Id { get; set; }
        [Required, RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿/\\\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$"), Display(Name = "First name")]
        public string FirstName { get; set; }
        [Required, RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿/\\\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$"), Display(Name = "Last name")]
        public string LastName { get; set; }
        [Required, Phone, Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        // booking info
        [Required, DataType(DataType.DateTime)]
        public DateTime BookingDate { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime DesiredDate { get; set; }
        [Required, DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }
        [Required]
        public TimeSpan EndTime { get; set; }
        public float TotalCost { get; set; }
        public float Paid { get; set; }
        [Required]
        public BookingStatus BookingStatus { get; set; }

        // relations
        [ForeignKey("SpaceEmployeeId")]
        public int EmployeeId { get; set; }
        [Required]
        public SpaceEmployee Employee { get; set; }

        [Required]
        public List<BookingHall> BookingHalls { get; set; }
        public Payment Payment { get; set; }

    }
}
