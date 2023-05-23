using GetSit.Data.enums;
using GetSit.Data.Security;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;
namespace GetSit.Models
{
    public class Customer:IAbstractUser
    {
        [Key]
        public int Id { get; set; }
        [Required, RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿/\\\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$"),Display(Name ="First name")]
        public string FirstName { get; set; }
        [Required, RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿/\\\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$"), Display(Name = "Last name")]
        public string LastName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(8), RegularExpression("%[A-Z]%",ErrorMessage ="at least one uppercase letter is required")]
        public string Password { get; set; }
        [Required,Phone, Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }
        [DataType(DataType.Url), AllowNull]
        public string ProfilePictureUrl { get; set; } = "resource/site/user-profile-icon.jpg";
        public CustomerType CustomerType { get; set; }
        [AllowNull]
        public string? Country { get; set; }
        [AllowNull]
        public string? City { get; set; }
        public int Penality { get; set; } = 0;
        [Required,DefaultValue(false)]
        public bool Blocked { get; set; }
        [AllowNull,ForeignKey("FacultyId")]
        public int? FacultyId { get; set; }
        public Faculty? Faculty { get; set; }
        [AllowNull,ForeignKey("TitleId")]
        public int? TitleId { get; set; }
        public Title? Title { get; set; }
        public List<PaymentCard>? PaymentCards { get; set; }
        public List<FavoriteHall>? FavoriteHalls { get; set; }
        public List<Booking>? Bookings { get; set; }

    }
}   
