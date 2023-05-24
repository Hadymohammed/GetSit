using GetSit.Data.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GetSit.Models
{
    public class SpaceWorkingDay
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public WeekDay Day { get; set; }
        [Required]
        public DateTime OpeningTime { get; set; }
        [Required]
        public DateTime ClosingTime { get; set; }
        [ForeignKey("SpaceId")]
        public int SpaceId { get; set; }
        [Required]
        public Space Space { get; set; }
    }
}
