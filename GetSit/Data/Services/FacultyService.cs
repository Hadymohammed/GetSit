using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public class FacultyService:EntityBaseRepository<Faculty>,IFacultyService
    {
        public FacultyService(AppDBcontext context):base(context)
        {

        }
    }
}
