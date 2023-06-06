using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public class Space_Service:EntityBaseRepository<Space>,ISpaceService
    {
        public Space_Service(AppDBcontext context):base(context)
        {

        }
    }
}
