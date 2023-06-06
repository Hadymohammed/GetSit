using GetSit.Data.Base;
using GetSit.Models;
using Microsoft.EntityFrameworkCore;

namespace GetSit.Data.Services
{
    public class SpaceHallService:EntityBaseRepository<SpaceHall>,ISpaceHallService
    {
        private readonly AppDBcontext _context;

        public SpaceHallService(AppDBcontext context):base(context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<SpaceHall>> GetBySpaceName(string Key)
        {
            var result = await _context.SpaceHall.Include(x => x.Space).Include(y => y.HallPhotos).Where(p => p.Space.Name.Contains(Key)).ToListAsync();
            return result;
        }
    }
}
