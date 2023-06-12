using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public class HallPhotoService:EntityBaseRepository<HallPhoto>, IHallPhotoService
    {
        AppDBcontext _context;
        public HallPhotoService(AppDBcontext context):base(context)
        {
            _context = context;
        }
        public List<HallPhoto> GetByHallId(int hallId)
        {
            return _context.HallPhoto.Where(p => p.HallId == hallId).ToList();
        }
    }
}
