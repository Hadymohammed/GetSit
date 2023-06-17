using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public class SpacePhotoService:EntityBaseRepository<SpacePhoto>,ISpacePhotoService
    {
        readonly AppDBcontext _context;
        public SpacePhotoService(AppDBcontext context):base(context)
        {
            _context = context;
        }

        public List<SpacePhoto> GetBySpaceId(int SpaceId)
        {
            return _context.SpacePhoto.Where(p => p.SpaceId == SpaceId).ToList();  
        }
    }
}
