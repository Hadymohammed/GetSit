using GetSit.Data.Base;
using GetSit.Data.ViewModels;
using GetSit.Models;

namespace GetSit.Data.Services
{
	public interface ISpaceService : IEntityBaseRepository<Space>
	{
        public List<CustomerWithTotalBookings> GetCustomersWithMostBookings(int spaceId, int count);

    }
}
