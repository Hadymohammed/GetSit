using GetSit.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace GetSit.Models
{
    public class Faculty : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Customer> Customers { get; set; }
    }
}
