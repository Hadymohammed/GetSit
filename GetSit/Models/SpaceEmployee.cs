using GetSit.Data.enums;
using GetSit.Data.Security;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GetSit.Models
{
    public class SpaceEmployee:IAbstractUser
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
        [Required]
        public string ProfilePictureUrl { get; set; } = "resource/site/user-profile-icon.jpg";
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public SpaceEmployeeRole EmployeeRole { get; set; }
        [ForeignKey("SpaceId")]
        public int SpaceId { get; set; }
        public Space? Space { get; set; }
    }
}
