using System.ComponentModel.DataAnnotations;

namespace GetSit.Data.ViewModels
{
    public class RejectHallVM
    {
        [Required]
        public int RequestId { get; set; }
        [Required]
        public string Messege { get; set; }
    }
}
