using GetSit.Data.enums;
using System.ComponentModel.DataAnnotations;

namespace GetSit.Models
{
    public class SpaceEmployee
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
        public string ProfilePictureUrl { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public SpaceEmployeeRole EmployeeRole { get; set; }
    }
}
