using GetSit.Data.enums;
using GetSit.Data.Validation;
using GetSit.Models;
using System.ComponentModel.DataAnnotations;

namespace GetSit.Data.ViewModels
{

    public class EditHallVM
    {
        [Required]
        public int SpaceId { get; set; }
        [Required]
        public string SpacePhotoUrl { get; set; }
        [Required]
        public string SpaceName { get; set; }
        [Required]
        public string SpaceBio { get; set; }
        [Required]
        public int HallId { get; set; }
        public int Capacity { get; set; }
        [Required]
        public float CostPerHour { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public HallType Type { get; set; }
        public List<HallPhoto>? HallPhotos { get; set; }
        public List<HallFacility>? HallFacilities { get; set; }
        public List<IFormFile>? Files { get; set; }
    }
}
