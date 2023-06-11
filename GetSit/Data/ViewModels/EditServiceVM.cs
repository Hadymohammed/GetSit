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
        public SpaceService Service { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}
