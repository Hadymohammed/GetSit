using GetSit.Data.enums;
using GetSit.Models;
using System.ComponentModel.DataAnnotations;

namespace GetSit.Data.ViewModels
{
    public class AddHallVM
    {
        [Required]
        public Space Space { get; set; }
        [Required]
        public int Capacity { get; set; }
        [Required]
        public float CostPerHour { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public HallType Type { get; set; }
        [Required]
        public List<IFormFile> Files { get; set; }
    }
}
