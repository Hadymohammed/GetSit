using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public class ServicePhotoService:EntityBaseRepository<ServicePhoto>,IServicePhotoService
    {
        public ServicePhotoService(AppDBcontext context):base(context)
        {

        }
    }
}
