using GetSit.Data.enums;
using System.ComponentModel.DataAnnotations;

namespace GetSit.Data.ViewModels
{
    public class LoginVM
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email address is required")]
        public string Email{ get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public UserRole Role { get; set; }
    }
}
