using GetSit.Data.Base;
using GetSit.Data.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace GetSit.Models
{
    public class SpaceHall : IEntityBase
    {
        [Key]
        public int Id { get; set; }

  [ForeignKey("SpaceId")]
        public int SpaceId { get; set; }
        public Space Space { get; set; }
        [Required]
        public int Capacity { get; set; }
        [Required]
        public float  CostPerHour { get; set; }
        [Required]
        public string  Description { get; set; }
        [Required]
        public HallStatus Status { get; set; }
        [Required]
        public HallType Type { get; set; }
        [AllowNull]
        public string? Thumbnail { get; set; }= "resource/site/user-profile-icon.jpg";
        [Required]
        public List<HallPhoto> HallPhotos { get; set; }
        [Required]
        public List<HallFacility> HallFacilities { get; set; }
        [Required]
        public List<FavoriteHall> FavoriteHalls { get; set; }
        [Required]
        public List<BookingHall> Bookings { get; set; }

    }
}
