using GetSit.Data.Base;
using GetSit.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GetSit.Data.Services
{
    public class SpaceHallService:EntityBaseRepository<SpaceHall>,ISpaceHallService
    {
        private readonly AppDBcontext _context;

        public SpaceHallService(AppDBcontext context):base(context)
        {
            _context = context;
        }

        public List<SpaceHall> GetBySpaceId(int spaceId, params Expression<Func<SpaceHall, object>>[] includeProperties)
        {
            IQueryable<SpaceHall> query = _context.Set<SpaceHall>();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return query.Where(n => n.SpaceId == spaceId).ToList();
        }

        public async Task<IEnumerable<SpaceHall>> GetBySpaceName(string Key)
        {
            var result = await _context.SpaceHall.Include(x => x.Space).Include(y => y.HallPhotos).Where(p => p.Space.Name.Contains(Key)).ToListAsync();
            return result;
        }
    }
}
