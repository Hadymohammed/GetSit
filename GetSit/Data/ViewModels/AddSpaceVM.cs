using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace GetSit.Data.ViewModels
{
    public class AddSpaceVM
    {
        [Required]
        public string SpaceName { get; set; }
        [Required]
        public string SpaceBio { get; set; }
        [Required]
        public string SpaceCountry { get; set; }
        [Required]
        public string SpaceCity { get; set; }
        [Required]
        public string SpaceStreet { get; set; }
        [Required]
        public string SpaceGPSLocation { get; set; }
        [Required, DefaultValue(true)]
        public bool IsFast { get; set; }
    }
}
