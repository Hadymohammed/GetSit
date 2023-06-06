using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public class SpacePhoneService:EntityBaseRepository<SpacePhone>,ISpacePhoneService
    {
        public SpacePhoneService(AppDBcontext context):base(context)
        {
            
        }
    }
}
