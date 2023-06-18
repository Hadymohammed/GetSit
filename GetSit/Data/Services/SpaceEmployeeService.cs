using GetSit.Data.Base;
using GetSit.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GetSit.Data.Services
{
    public class SpaceEmployeeService:EntityBaseRepository<SpaceEmployee>,ISpaceEmployeeService
    {
        private readonly AppDBcontext _context;
        public SpaceEmployeeService(AppDBcontext context):base(context)
        {
            _context = context;
        }
        public List<SpaceEmployee> GetBySpaceId(int spaceId, params Expression<Func<SpaceEmployee, object>>[] includeProperties)
        {
            IQueryable<SpaceEmployee> query = _context.Set<SpaceEmployee>();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return query.Where(n => n.SpaceId == spaceId).ToList();
        }
        public SpaceEmployee? GetByEmail(string email)
        {
            return _context.SpaceEmployee.Where(e => e.Email == email).FirstOrDefault();
        }
    }
}
