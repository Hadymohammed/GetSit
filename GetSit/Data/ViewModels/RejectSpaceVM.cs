using System.ComponentModel.DataAnnotations;

namespace GetSit.Data.ViewModels
{
    public class RejectSpaceVM
    {
        [Required]
        public int SpaceId { get; set; }
        [Required]
        public string Messege { get; set; }
    }
}
