using GetSit.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GetSit.Models
{
    public class SpacePhone : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [ForeignKey("SpaceId")]
        public int SpaceId { get; set; }
        [Required]
        public Space Space { get; set; }
    }
}
