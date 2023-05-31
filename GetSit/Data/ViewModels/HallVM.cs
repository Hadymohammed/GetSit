using GetSit.Models;
namespace GetSit.Data.ViewModels
{
    public class HallVM
    {
        public SpaceHall Hall { get; set; }
        public IEnumerable<IFormFile> Files { get; set; }
    }
}
