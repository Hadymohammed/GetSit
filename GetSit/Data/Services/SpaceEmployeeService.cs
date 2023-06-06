using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public class SpaceEmployeeService:EntityBaseRepository<SpaceEmployee>,ISpaceEmployeeService
    {
        public SpaceEmployeeService(AppDBcontext context):base(context)
        {

        }
    }
}
