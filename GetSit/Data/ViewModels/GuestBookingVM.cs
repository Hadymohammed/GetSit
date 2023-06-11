using GetSit.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace GetSit.Data.ViewModels
{
    public class GuestBookingVM
    {

        [Required, RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿/\\\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$"), Display(Name = "First name")]
        public string FirstName { get; set; }
        [Required, RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿/\\\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$"), Display(Name = "Last name")]
        public string LastName { get; set; }
        [Required, Phone, Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
        [Required]
        public SpaceHall SelectedHall { get; set; }
        [Required]
        public Space SelectedSpace { get; set; }
        [Required]
        public List<Tuple<TimeSpan, bool>>? AvailableSlots { get; set; }
        [AllowNull]
        public Dictionary<int, int> SelectedServicesQuantities { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime BookingDate { get; set; }
        public DateTime FilterDate { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime DesiredDate { get; set; }
        [Required, DataType(DataType.Date)]
        public TimeSpan StartTime { get; set; }
        [Required, DataType(DataType.Date)]
        public TimeSpan EndTime { get; set; }
        [Required]
        public float TotalCost { get; set; }
        public List<Dictionary<DateTime, List<Tuple<TimeSpan, bool>>>>? SlotsForWeek { get; set; }
    }
}
