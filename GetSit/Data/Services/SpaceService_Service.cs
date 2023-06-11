using GetSit.Data.Base;
using GetSit.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GetSit.Data.Services
{
    public class SpaceService_Service:EntityBaseRepository<SpaceService>,ISpaceService_Service
    {
        readonly AppDBcontext _context;
        public SpaceService_Service(AppDBcontext context):base(context)
        {
            _context = context;
        }
        public List<SpaceService> GetBySpaceId(int spaceId, params Expression<Func<SpaceService, object>>[] includeProperties)
        {
            IQueryable<SpaceService> query = _context.Set<SpaceService>();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return query.Where(n => n.SpaceId == spaceId).ToList();
        }
    }
}
