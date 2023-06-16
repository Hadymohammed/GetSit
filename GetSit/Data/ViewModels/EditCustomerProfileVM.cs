using GetSit.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace GetSit.Data.ViewModels
{
    public class EditCustomerProfileVM
    {
        public int Id { get; set; }
        [Required, MinLength(8),
         RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$", ErrorMessage = "Must be at least 8 characters and contain at least one letter and one number.")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [AllowNull]
        public string Country { get; set; }
        [AllowNull]
        public string? City { get; set; }
        [DataType(DataType.Url), AllowNull]
        public string ProfilePictureUrl { get; set; } = "~/resources/site/user.jpg";

        [Required, Phone, Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Name { get; set; }
        
        public Title Title { get; set; }

        [Required]
        public string userName { get; set; }
        public string Description { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public Faculty? Faculty { get; set; }

        public String? Bio { get; set; }
    }
}
