using GetSit.Data.Base;
using GetSit.Data.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace GetSit.Models
{
    public class SpaceContact : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public SpaceContacts ContactType { get; set; }
        public string Contact { get; set; }
        [Required, ForeignKey("SpaceId")]
        public int SpaceId { get; set; }
        public Space? Space { get; set; }
    }
}