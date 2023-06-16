using GetSit.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GetSit.Models
{
    public class FavoriteHall : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        [Required]  
        public int CustomerId { get; set; }
        [Required]
        public Customer  customer { get; set; }
        [ForeignKey("HallId")]
        public int HallId { get; set; }
        
        [Required]
        public SpaceHall SpaceHall { get; set; }


    }
}
