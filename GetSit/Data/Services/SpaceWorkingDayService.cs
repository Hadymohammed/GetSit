using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public class SpaceWorkingDayService:EntityBaseRepository<SpaceWorkingDay>,ISpaceWorkingDayService
    {
        public SpaceWorkingDayService(AppDBcontext context):base(context)
        {
            
        }
    }
}
