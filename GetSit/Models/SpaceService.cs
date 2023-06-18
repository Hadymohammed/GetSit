using GetSit.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace GetSit.Models
{
    public class SpaceService : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public string Description { get; set; }
        [AllowNull]
        public string? Thumbnail { get; set; } = "resource/site/user-profile-icon.jpg";
        [Required]
        public List<ServicePhoto> ServicePhotos { get; set; }
        [ForeignKey("SpaceId")]
        public int SpaceId { get; set; }
        [Required]
        public Space Space { get; set; }
        [Required]
        public List<BookingHallService> Bookings { get; set; }
    }
}
