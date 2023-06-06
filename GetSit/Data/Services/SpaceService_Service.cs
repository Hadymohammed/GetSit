using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public class SpaceService_Service:EntityBaseRepository<SpaceService>,ISpaceService_Service
    {
        public SpaceService_Service(AppDBcontext context):base(context)
        {

        }
    }
}
