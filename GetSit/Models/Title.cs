using GetSit.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace GetSit.Models
{
    public class Title : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public List<Customer> Customers { get; set; }
    }
}
