using GetSit.Data.Base;
using GetSit.Models;
using System.Linq.Expressions;

namespace GetSit.Data.Services
{
    public interface ISpaceService_Service:IEntityBaseRepository<SpaceService>
    {
        public List<SpaceService> GetBySpaceId(int spaceId, params Expression<Func<SpaceService, object>>[] includeProperties);

    }
}
