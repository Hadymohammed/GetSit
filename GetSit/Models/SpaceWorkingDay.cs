using GetSit.Data.Base;
using GetSit.Data.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GetSit.Models
{
    public class SpaceWorkingDay : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DayOfWeek Day { get; set; }
        [Required]
        public TimeSpan OpeningTime { get; set; }
        [Required]
        public TimeSpan ClosingTime { get; set; }
        [ForeignKey("SpaceId")]
        public int SpaceId { get; set; }
        [Required]
        public Space Space { get; set; }
    }
}
