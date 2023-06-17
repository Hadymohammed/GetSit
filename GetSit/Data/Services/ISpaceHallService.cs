using GetSit.Data.Base;
using GetSit.Models;
using System.Linq.Expressions;

namespace GetSit.Data.Services
{
    public interface ISpaceHallService:IEntityBaseRepository<SpaceHall>
    {
        public Task<IEnumerable<SpaceHall>> GetBySpaceName(String Key);
        public List<SpaceHall> GetBySpaceId(int spaceId, params Expression<Func<SpaceHall, object>>[] includeProperties);
    }
}
