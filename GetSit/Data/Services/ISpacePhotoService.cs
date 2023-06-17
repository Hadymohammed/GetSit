using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public interface ISpacePhotoService:IEntityBaseRepository<SpacePhoto>
    {
        public List<SpacePhoto> GetBySpaceId(int SpaceId);
    }
}
