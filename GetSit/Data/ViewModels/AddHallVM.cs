using GetSit.Data.enums;
using GetSit.Models;
using System.ComponentModel.DataAnnotations;
using GetSit.Data.Validation;


namespace GetSit.Data.ViewModels
{
    public class AddHallVM
    {
        [Required]
        public int SpaceId { get; set; }
        [Required]
        public string SpacePhotoUrl { get; set; }
        [Required]
        public string SpaceName { get; set; }
        [Required]
        public string SpaceBio { get; set; }
        [Required,NotZero(ErrorMessage ="Capacity cann't be Zero!")]
        public int Capacity { get; set; }
        [Required, NotZero(ErrorMessage = "Cost cann't be Zero!")]
        public float CostPerHour { get; set; }
        [Required,MaxLength(200,ErrorMessage ="Max length is 200 character")]
        public string Description { get; set; }
        [Required]
        public HallType Type { get; set; }
        [Required,MinLength(3,ErrorMessage ="Atleast 3 photos required")]
        public List<IFormFile> Files { get; set; }
        public IFormFile Thumbnail { get; set; }
    }
}
