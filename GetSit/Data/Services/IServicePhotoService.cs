using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public interface IServicePhotoService : IEntityBaseRepository<ServicePhoto>
    {
        public List<ServicePhoto> GetByServiceId(int ServiceId);
    }
}
