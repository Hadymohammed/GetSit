using GetSit.Data.Base;
using GetSit.Models;
using System.Linq.Expressions;

namespace GetSit.Data.Services
{
    public interface ISpaceEmployeeService:IEntityBaseRepository<SpaceEmployee>
    {
        public SpaceEmployee GetByEmail(string email);
        public List<SpaceEmployee> GetBySpaceId(int spaceId, params Expression<Func<SpaceEmployee, object>>[] includeProperties);
    }
}
