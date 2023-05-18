using GetSit.Data.enums;
using GetSit.Data.Validation;
using System.ComponentModel.DataAnnotations;

namespace GetSit.Data.ViewModels
{
    public class LoginVM
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Must be a valid email")]
        public string Email{ get; set; }

        [Required]
        [DataType(DataType.Password),MinLength(8,ErrorMessage ="Must be at least 8 length")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Role is required")]
        [UserRoleValidator(ErrorMessage = "Must be an Admin or Customer or Provider.")]
        public UserRole Role { get; set; }
    }
}
