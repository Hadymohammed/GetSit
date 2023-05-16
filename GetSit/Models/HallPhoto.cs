using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GetSit.Models
{
    public class HallPhoto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Url { get; set; }
        [ForeignKey("HallId")]
        public int HallId { get; set; }
        [Required]
        public SpaceHall? Hall { get; set; }


    }
}
