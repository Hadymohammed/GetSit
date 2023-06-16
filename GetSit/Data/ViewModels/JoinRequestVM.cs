using GetSit.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GetSit.Data.ViewModels
{
    public class JoinRequestVM
    {
        // manager info
        [Required, RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿/\\\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$"), Display(Name = "First name")]
        public string FirstName { get; set; }
        [Required, RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿/\\\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$"), Display(Name = "Last name")]
        public string LastName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, Phone]
        public string PhoneNumber { get; set; }
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }

        //space data
        public string SpaceName { get; set; }
        [Required]
        public string SpaceBio { get; set; }
        [Required]
        public string SpaceCountry { get; set; }
        [Required]
        public string SpaceCity { get; set; }
        [Required]
        public string SpaceStreet { get; set; }
        [Required]
        public string SpaceGPSLocation { get; set; }
        [Required, DefaultValue(true)]
        public bool IsFast { get; set; }
    }
}
