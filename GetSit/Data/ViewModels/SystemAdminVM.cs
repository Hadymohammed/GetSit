using GetSit.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace GetSit.Data.ViewModels
{
    public class SystemAdminVM
    {
        [Required]
        List<Tuple<int, int>>? ProviderRequest { get; set; } 
        [Required]
        List<Tuple<int, int>>? HallRequest { get; set; }
        [Required]
        List<Tuple<int, int>>? ServiceRequest { get; set; } 


    }
}
