using Microsoft.AspNetCore.Mvc;
using GetSit.Data.ViewModels;
using GetSit.Data;
using GetSit.Models;

namespace GetSit.Controllers
{
    public class SpaceController : Controller
    {
        public readonly AppDBcontext _context;
        private object _webHastEnvronment;

        public SpaceController(AppDBcontext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddHall(/*int SpaceId*/)
        {
            //if (SpaceId == null) { return NotFound(); }
            int SpaceId = 1;
            HallVM vm = new HallVM();
            var sp = _context.Space.Where(x => x.Id == SpaceId).FirstOrDefault();
            var hall = new SpaceHall()
            {
                Space = sp,
                SpaceId = SpaceId,
            };
            vm.Hall = hall;

            return View(vm);
        }
        [HttpPost,ActionName("AddHall")]
        public IActionResult AddHall(HallVM vm)
        {
            var temp = new List<HallPhoto>();
                foreach (var file in vm.Files)
                {
                    if (file != null && file.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/resources/HallPhotos", fileName);
                        var tmp = new HallPhoto()
                        {
                            Url = filePath,
                        };
                        temp.Add(tmp);
                        using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                        {
                              file.CopyToAsync(fileSrteam);
                        }
                    }
                }
                
             vm.Hall.HallPhotos = temp;
            
            //_context.SpaceHall.Add(vm.Hall);
            //_context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
