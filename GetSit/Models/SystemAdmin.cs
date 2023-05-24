using GetSit.Data.enums;
using GetSit.Data.Security;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace GetSit.Models
{
    public class SystemAdmin:IAbstractUser
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required, Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }
        [Required, AllowNull]
        public string ProfilePictureUrl { get; set; } = "resource/site/user-profile-icon.jpg";
        [Required, AllowNull]
        public string Country { get; set; }
        [Required, AllowNull]
        public string City { get; set; }
        [Required]
        public SystemAdminRole AdminRole { get; set; } = SystemAdminRole.Super;
    }
}
