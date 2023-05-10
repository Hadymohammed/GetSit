using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GetSit.Models
{
    public class PaymentCard
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string CardNumber { get; set; }
        [Required]
        public DateTime ExpireDate { get; set; }
        [Required]
        public string CCV { get; set; }
        [Required]
        public Customer Customer { get; set; }
        [ForeignKey("CustomerId")]
        public int CustomerId { get; set; }

    } 
}
