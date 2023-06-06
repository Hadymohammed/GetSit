using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public class TitleService:EntityBaseRepository<Title>,ITitleService
    {
        public TitleService(AppDBcontext context):base(context)
        {

        }
    }
}
