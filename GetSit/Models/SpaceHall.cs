using GetSit.Data.enums;
using System.ComponentModel.DataAnnotations;

namespace GetSit.Models
{
    public class SpaceHall
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
