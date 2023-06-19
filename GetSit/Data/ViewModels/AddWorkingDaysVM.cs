using GetSit.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace GetSit.Data.ViewModels
{
    public class AddWorkingDaysVM
    {
        [AllowNull]
        public int? Id { get; set; }
        [Required]
        public DayOfWeek Day { get; set; }
        [Required]
        public string OpeningTime { get; set; }
        [Required]
        public string ClosingTime { get; set; }
    }
}
