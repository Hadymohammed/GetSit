using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public interface ISpaceHallService:IEntityBaseRepository<SpaceHall>
    {
        public Task<IEnumerable<SpaceHall>> GetBySpaceName(String Key);

    }
}
