using System.ComponentModel.DataAnnotations;

namespace GetSit.Data.Security
{
    public interface IAbstractUser
    {
        public int Id { get; set; }
        [Required,EmailAddress]
        public string Email { get; set; }

    }
}
