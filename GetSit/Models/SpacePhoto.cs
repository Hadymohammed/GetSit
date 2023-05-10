using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GetSit.Models
{
    public class SpacePhoto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string  Url { get; set; }
        [ForeignKey("SpaceId")]
        public  int SpaceId { get; set; }
        [Required]
        public  Space Space { get; set; }
    }
}
