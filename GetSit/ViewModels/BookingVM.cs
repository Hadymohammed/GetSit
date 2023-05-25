using GetSit.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace GetSit.ViewModels
{
    public class BookingVM
        
    {
        [Required]
        public SpaceHall SelectedHall { get; set; }
        [Required]
        public Space SelectedSpace { get; set; }
        [Required]
        public List <TimeSpan> AvailableSlots{ get; set; }
        [AllowNull]
        public Dictionary<int, int> SelectedServicesQuantities { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime BookingDate { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime DesiredDate { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime StartTime { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime EndTime { get; set; }
        [Required]
        public float TotalCost { get; set; }
    }
}
