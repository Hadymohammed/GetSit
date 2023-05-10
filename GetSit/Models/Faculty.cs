using System.ComponentModel.DataAnnotations;

namespace GetSit.Models
{
    public class Faculty
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Customer> Customers { get; set; }
    }
}
