using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public class ServicePhotoService:EntityBaseRepository<ServicePhoto>,IServicePhotoService
    {
        public ServicePhotoService(AppDBcontext context):base(context)
        {

        }

        public string GetPhotoFileName(int photoId)
        {
            throw new NotImplementedException();
        }

        public Task<IFormFile> GetThumbnailAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateThumbnailAsync(int id, string result)
        {
            throw new NotImplementedException();
        }
    }
}
