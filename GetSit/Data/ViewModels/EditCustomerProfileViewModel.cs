using GetSit.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace GetSit.Models
{
    public class EditCustomerProfileViewModel
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
        public string ProfilePictureUrl { get; set; }
        [Required, Phone, Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

    }
}
