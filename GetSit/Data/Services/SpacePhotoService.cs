using GetSit.Data.Base;
using GetSit.Models;
using Microsoft.EntityFrameworkCore;

namespace GetSit.Data.Services
{
    public class SpacePhotoService:EntityBaseRepository<SpacePhoto>,ISpacePhotoService
    {
        readonly AppDBcontext _context;
        public SpacePhotoService(AppDBcontext context):base(context)
        {
            _context = context;
        }
        public List<SpacePhoto> GetBySpaceId(int spaceId)
        {
            return _context.SpacePhoto.Where(s => s.SpaceId == spaceId).ToList();
        }
    }

}
