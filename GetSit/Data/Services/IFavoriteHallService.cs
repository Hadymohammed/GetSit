using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public interface IFavoriteHallService:IEntityBaseRepository<FavoriteHall>
    {
        public FavoriteHall GetByHallIdAndUserId(int HId, int CId);
        public Task ToggleAysnc(int HId, int CId);

    }
}
