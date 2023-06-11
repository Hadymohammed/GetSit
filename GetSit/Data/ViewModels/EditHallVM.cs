using GetSit.Data.enums;
using GetSit.Data.Validation;
using GetSit.Models;
using System.ComponentModel.DataAnnotations;

namespace GetSit.Data.ViewModels
{

    public class EditHallVM
    {
        [Required]
        public SpaceHall Hall { get; set; }
        [Required]
        public int SpaceId { get; set; }
        [Required]
        public string SpacePhotoUrl { get; set; }
        [Required]
        public string SpaceName { get; set; }
        [Required]
        public string SpaceBio { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}
