using GetSit.Models;

namespace GetSit.Data.Services
{
    public interface IExploreService
    {
        Task<IEnumerable<SpaceHall>> GetAll();
        Task<IEnumerable<SpaceHall>> GetBySearch(String Key);
        Space GetById(int id);
        void DeleteById(int id);
        Space UpdateById(int id , SpaceHall SpaceHall);
        void Add(SpaceHall SpaceHall);
        void Fav(int Hid, int CId);

    }
}
