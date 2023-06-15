using GetSit.Data.Base;
using GetSit.Models;
using System.Linq.Expressions;

namespace GetSit.Data.Services
{
    public interface ISpacePhotoService:IEntityBaseRepository<SpacePhoto>
    {
        public List<SpacePhoto> GetBySpaceId(int spaceId);

    }
}
