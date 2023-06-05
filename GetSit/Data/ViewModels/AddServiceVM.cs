using GetSit.Data.enums;
using GetSit.Data.Validation;
using System.ComponentModel.DataAnnotations;

namespace GetSit.Data.ViewModels
{
    public class AddServiceVM
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
        public string ServiceName { get; set; }
        [Required, NotZero(ErrorMessage = "Cost cann't be Zero!")]
        public float CostPerUnit { get; set; }
        [Required, MaxLength(200, ErrorMessage = "Max length is 200 character")]
        public string Description { get; set; }
        [Required, MinLength(1, ErrorMessage = "At least 1 photo required")]
        public List<IFormFile> Files { get; set; }
        public IFormFile Thumbnail { get; set; }
    }
}
