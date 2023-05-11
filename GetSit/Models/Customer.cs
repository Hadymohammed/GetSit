using GetSit.Data.enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace GetSit.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required, RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿/\\\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$")]
        public string FirstName { get; set; }
        [Required, RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿/\\\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$")]
        public string LastName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(8), RegularExpression("%[A-Z]%",ErrorMessage ="at least one uppercase letter is required")]
        public string Password { get; set; }
        [Required,Phone]
        public string PhoneNumber { get; set; }
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }
        [DataType(DataType.Url)]
        public string ProfilePictureUrl { get; set; }
        [Required]
        public CustomerType CustomerType { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int Penality { get; set; }
        [Required,DefaultValue(false)]
        public bool Blocked { get; set; }
        public int FacultyId { get; set; }
        [ForeignKey("FacultyId")]
        public Faculty Faculty { get; set; }
        public int TitleId { get; set; }
        [ForeignKey("TitleId")]
        public Title Title { get; set; }
        public List<PaymentCard> PaymentCards { get; set; }
        public List<FavoriteHall> FavoriteHalls { get; set; }
        public List<Booking> Bookings { get; set; }

    }
}   
