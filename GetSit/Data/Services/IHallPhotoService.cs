using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public interface IHallPhotoService : IEntityBaseRepository<HallPhoto>
    {
        string GetPhotoFileName(int photoId);
        Task<string> GetThumbnailUrlAsync(int id);
        Task UpdatePhotoAsync(int id, int cnt, string result);
        Task UpdateThumbnailAsync(int id, string result);
    }
}
