using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace GetSit.Data.ViewModels
{
    public class CreateCustomerBookingVM
    {
        [Required]
        public int HallId { get; set; }
        [Required]
        public int SpaceId { get; set; }
        public Dictionary<int, int>? SelectedServicesQuantities { get; set; }
        [Required]
        public DateTime DesiredDate { get; set; }
        [Required]
        public string StartTime { get; set; }
        [Required]
        public string EndTime { get; set; }

    }
}
