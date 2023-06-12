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
        [Required,AllowNull]
        public string ?BankAccount { get; set; }
        public IEnumerable<SpacePhoto> Photos  { get; set; }
        [Required]
        public List<SpacePhone> Phones { get; set; }
        [Required]
        public List<SpaceWorkingDay> WorkingDays { get; set; }
        [Required]
        public List<SpaceService> Services { get; set; }
        [Required]
        public ICollection<SpaceHall> ?Halls { get; set; }
        public List<SpaceEmployee>? Employees { get; set; }

        [Required, DefaultValue(false)]
        public bool IsApproved { get; set; }

    }
}
