using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public class HallPhotoService:EntityBaseRepository<HallPhoto>, IHallPhotoService
    {
        public HallPhotoService(AppDBcontext context):base(context)
        {

        }

        public string GetPhotoFileName(int photoId)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetThumbnailUrlAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePhotoAsync(int id, int cnt, string result)
        {
            throw new NotImplementedException();
        }

        public Task UpdateThumbnailAsync(int id, string result)
        {
            throw new NotImplementedException();
        }
    }
}
