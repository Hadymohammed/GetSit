using GetSit.Data.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace GetSit.Models
{
    public class Space : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Bio { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string GPSLocation { get; set; }
        [Required,DefaultValue(true)]
        public bool IsFast { get; set; }
        [AllowNull]
        public string ?BankAccount { get; set; 
        [Required, DefaultValue(false)]
        public bool IsApproved { get; set; }
        [AllowNull,DefaultValue("resource/site/logo-social.png")]
        public string? SpaceLogo { get; set; }
        [AllowNull, DefaultValue("resource/site/Cover_PlaceHolder.png")]
        public string? SpaceCover { get; set; }
        [AllowNull]
        public string? Email { get; set; }
        [AllowNull]
        public string? Facebook { get; set; }
        [AllowNull]
        public string? Twitter { get; set; }
        [AllowNull]
        public string? Instagram { get; set; }
        public List<SpacePhoto>? Photos  { get; set; }
        public List<SpacePhone>? Phones { get; set; }
        public List<SpaceWorkingDay>? WorkingDays { get; set; }
        public List<SpaceService>? Services { get; set; }
        public List<SpaceHall>? Halls { get; set; }
        public List<SpaceEmployee>? Employees { get; set; }
    }
}
