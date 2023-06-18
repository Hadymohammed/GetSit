using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
	public interface ISpaceService : IEntityBaseRepository<Space>
	{
		void UpdateSpace(Space space);
	}
}
