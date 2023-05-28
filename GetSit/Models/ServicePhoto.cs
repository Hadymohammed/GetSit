using GetSit.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GetSit.Models
{
    public class ServicePhoto : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Url { get; set; }
        [ForeignKey("ServiceId")]
        public int ServiceId { get; set; }
        [Required]
        public SpaceService SpaceService { get; set; }

    }
}
