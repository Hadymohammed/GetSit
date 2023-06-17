using GetSit.Models;
using System.ComponentModel.DataAnnotations;

namespace GetSit.Data.ViewModels
{
    public class EditServiceVM
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
        public int ServiceId { get; set; }
        [Required]
        public string ServiceName { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public string Description { get; set; }
        public List<ServicePhoto>? ServicePhotos { get; set; }
        public List<IFormFile>? Files { get; set; }
    }
}
