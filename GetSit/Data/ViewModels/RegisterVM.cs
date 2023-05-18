using GetSit.Data.enums;
using System.ComponentModel.DataAnnotations;

namespace GetSit.Data.ViewModels
{
    public class RegisterVM
    {
        [Display(Name = "First name")]
        [Required(ErrorMessage = "First name is required")]
        [RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿/\\\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$",ErrorMessage ="Must be a name")]
        public string FirstName { get; set; }
        [Display(Name = "Last name")]
        [Required(ErrorMessage = "Last name is required")]
        [ RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿/\\\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$", ErrorMessage = "Must be a name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Birthdate is required")]
        [DataType(DataType.Date,ErrorMessage ="Must be a valid date")]
        public DateTime Birthdate { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage ="Must be a valid email")]
        public string Email { get; set; }

        [Display(Name = "Phone number")]
        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage ="Must be a valid number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [DataType(DataType.Password), MinLength(8,ErrorMessage ="Must be atleast 8 length"), RegularExpression("%[A-Z]%", ErrorMessage = "at least one uppercase letter is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public UserRole Role { get; set; }
    }
}
