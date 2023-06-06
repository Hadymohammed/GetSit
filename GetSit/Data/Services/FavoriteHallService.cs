using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public class FavoriteHallService:EntityBaseRepository<FavoriteHall>,IFavoriteHallService
    {
        public FavoriteHallService(AppDBcontext context):base(context)
        {

        }
    }
}
