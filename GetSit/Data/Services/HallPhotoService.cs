using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public class HallPhotoService:EntityBaseRepository<HallPhoto>, IHallPhotoService
    {
        public HallPhotoService(AppDBcontext context):base(context)
        {

        }
    }
}
