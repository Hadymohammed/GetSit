using GetSit.Data.Base;
using GetSit.Models;
using System.Linq.Expressions;

namespace GetSit.Data.Services
{
    public interface ISpacePhoneService:IEntityBaseRepository<SpacePhone>
    {
        public List<SpacePhone> GetBySpaceId(int spaceId);

    }
}
