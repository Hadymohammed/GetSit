using GetSit.Data.Validation;
using System.ComponentModel.DataAnnotations;

namespace GetSit.Data.ViewModels
{
    public class EditBookingPostVM
    {
        [Required]
        public int HallId { get; set; }
        [Required]
        public int BookingId { get; set; }
        [Required]
        public string EndTime { get; set; }
        [Required]
        [RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿/\\\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$", ErrorMessage = "Must be a name")]
        public string FirstName { get; set; }
        [Required]
        [RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿/\\\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$", ErrorMessage = "Must be a name")]
        public string LastName { get; set; }
        [Required]
        [PhoneValidator(ErrorMessage = "Must be 11 length and starts with 011,010,012,015")]
        public string PhoneNumber { get; set; }
        public Dictionary<int, int>? SelectedServicesQuantities { get; set; }

    }
}
