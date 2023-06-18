using GetSit.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace GetSit.Data.ViewModels
{
    public class SystemAdminVM
    {
        [Required]
        public List <Tuple < Space , SpaceEmployee>> ?Spaces { get; set; }
        [Required]
        public List<Tuple<HallRequest , Space>>? hallRequest { get; set; }
        [Required]
        public int NumberOfCustomers  { get; set; }
        [Required]
        public int NumberOfSpaces { get; set; }
        [Required]
        public int NumberOfGuestBookings { get; set; }
        [Required]
        public int NumberOfBookings { get; set; }
        [Required]
        public SpaceEmployee SpaceEmployee { get; set; }
    }
}
