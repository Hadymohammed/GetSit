using GetSit.Data.Validation;
using GetSit.Models;

namespace GetSit.Data.ViewModels
{
    public class SpaceDetailsVM
    {
        public Space? Space { get; set; }
        public IFormFile? Cover { get; set; }
        public IFormFile? Logo { get; set; }
        public List<string>? NewPhones { get; set; }

    }
}
