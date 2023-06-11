using GetSit.Data.enums;
using GetSit.Data.Validation;
using GetSit.Models;
using System.ComponentModel.DataAnnotations;

namespace GetSit.Data.ViewModels
{
    public class EditHallVM 
    {
        public int Id { get; set; }
        public string? ThumbnailUrl { get; set; }
        public SpaceHall SpaceHall { get; set; }
        [Required]
        public int SpaceId { get; set; }
        [Required]
        public string SpacePhotoUrl { get; set; }
        [Required]
        public string SpaceName { get; set; }
        [Required]
        public string SpaceBio { get; set; }
        [Required, NotZero(ErrorMessage = "Cost cann't be Zero!")]
        public float CostPerHour { get; set; }
        [Required, MaxLength(200, ErrorMessage = "Max length is 200 character")]
        public string Description { get; set; }
        [Required]
        public HallType Type { get; set; }
        [Required, MinLength(3, ErrorMessage = "Atleast 3 photos required")]
        public List<Facility>? Facilities { get; set; }
        public IFormFile Thumbnail { get; set; }
        public List<IFormFile> Files { get; set; }

    }
}
