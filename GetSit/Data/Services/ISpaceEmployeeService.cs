using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public interface ISpaceEmployeeService:IEntityBaseRepository<SpaceEmployee>
    {
        public SpaceEmployee GetByEmail(string email);

    }
}
