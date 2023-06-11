using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public interface IServicePhotoService : IEntityBaseRepository<ServicePhoto>
    {
        string GetPhotoFileName(int photoId);
        Task<IFormFile> GetThumbnailAsync(int id);
        Task UpdateThumbnailAsync(int id, string result);
    }
}
