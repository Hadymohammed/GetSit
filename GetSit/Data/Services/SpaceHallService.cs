using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public class SpaceHallService:EntityBaseRepository<SpaceHall>,ISpaceHallService
    {
        public SpaceHallService(AppDBcontext context):base(context)
        {

        }
    }
}
