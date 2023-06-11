using GetSit.Data.Base;
using GetSit.Data.enums;
using GetSit.Data.Security;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace GetSit.Models
{
    public class SpaceEmployee:IAbstractUser,IEntityBase
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
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }
        [Required, AllowNull]
        public string ProfilePictureUrl { get; set; } = "resource/site/user-profile-icon.jpg";
        [AllowNull]
        public string? Country { get; set; }
        [AllowNull]
        public string? City { get; set; }
        [Required]
        public SpaceEmployeeRole EmployeeRole { get; set; } = SpaceEmployeeRole.Super;
        [ForeignKey("SpaceId"), AllowNull]
        public int? SpaceId { get; set; }
        public Space? Space { get; set; }
    }
}
