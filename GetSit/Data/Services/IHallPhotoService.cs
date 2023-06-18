using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public interface IHallPhotoService : IEntityBaseRepository<HallPhoto>
    {
        public List<HallPhoto> GetByHallId(int hallId);
    }
}
