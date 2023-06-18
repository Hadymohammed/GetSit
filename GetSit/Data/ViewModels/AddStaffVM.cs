using GetSit.Data.Validation;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace GetSit.Data.ViewModels
{
    public class AddStaffVM
    {
        [AllowNull]
        public int? SpaceId { get; set; }
        [AllowNull]
        public string? SpaceBio { get; set; }
        [AllowNull]
        public string? SpacePicUrl { get; set; }
        [AllowNull]
        public int? SpaceEmployeeId { get; set; }
        [AllowNull]
        public string? SpaceName { get; set; }

        [Display(Name = "First name")]
        [Required(ErrorMessage = "First name is required")]
        [RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿/\\\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$", ErrorMessage = "Must be a name")]
        public string FirstName { get; set; }
        [Display(Name = "Last name")]
        [Required(ErrorMessage = "Last name is required")]
        [RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿/\\\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$", ErrorMessage = "Must be a name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Birthdate is required")]
        [DataType(DataType.Date, ErrorMessage = "Must be a valid date")]
        [BirthDateValidator(ErrorMessage = "You must be +18")]
        public DateTime Birthdate { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Must be a valid email")]
        public string Email { get; set; }

        [Display(Name = "Phone number")]
        [Required(ErrorMessage = "Phone number is required")]
        [PhoneValidator(ErrorMessage = "Must be 11 length and starts with 011,010,012,015")]
        public string PhoneNumber { get; set; }
    }
}
