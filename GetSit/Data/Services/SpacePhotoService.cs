using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public class SpacePhotoService:EntityBaseRepository<SpacePhoto>,ISpacePhotoService
    {
        public SpacePhotoService(AppDBcontext context):base(context)
        {

        }
    }
}
