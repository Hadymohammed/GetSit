using GetSit.Data.enums;
using GetSit.Data.Security;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace GetSit.Data.ViewModels
{
    public class ForgetPasswordVM
    {
        [Required,EmailAddress]
        public string Email { get; set; }
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d@#$%^&+=]{8,}$", ErrorMessage = "Must be at least 8 characters and contain at least one letter and one number.")]
        public string? Password { get; set; }
        [Required]
        public UserRole Role { get; set; }
    }
}
