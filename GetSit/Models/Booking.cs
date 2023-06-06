using GetSit.Data.Base;
using GetSit.Data.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace GetSit.Models
{
    public class Booking:IEntityBase
    {
        [Key]
        public int Id { get; set; }
        [Required, DataType(DataType.DateTime)]
        //? Error : DateTime & DateTime
        public DateTime BookingDate { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime DesiredDate { get; set; }
        [Required, DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }
        [Required]
        public float NumberOfHours { get; set; }
        public float TotalCost { get; set; }
        public float Paid { get; set; }
        [Required]
        public BookingStatus BookingStatus { get; set; }
        [Required]
        public BookingType BookingType { get; set; }
        public Payment Payment { get; set; }
        [AllowNull, ForeignKey("CustomerId")]
        public int CustomerId { get; set; }
        [Required]
        public Customer Customer { get; set; }
        [Required]
        public List<BookingHall> BookingHalls { get; set; }


    }
}