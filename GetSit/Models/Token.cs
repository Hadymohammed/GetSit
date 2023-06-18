using System.ComponentModel.DataAnnotations;

namespace GetSit.Models
{
    public class Token
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string token { get; set; }
    }
}
