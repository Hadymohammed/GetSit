using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public class ServicePhotoService:EntityBaseRepository<ServicePhoto>,IServicePhotoService
    {
        AppDBcontext _context;
        public ServicePhotoService(AppDBcontext context):base(context)
        {
            _context = context;
        }
        public List<ServicePhoto> GetByServiceId(int ServiceId)
        {
            return _context.ServicePhoto.Where(p=>p.ServiceId== ServiceId).ToList();
        }

    }
}
