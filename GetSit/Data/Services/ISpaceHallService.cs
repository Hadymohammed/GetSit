using GetSit.Models;

namespace GetSit.Data.Services
{
    public interface ISpaceHallService
    {
        Task<IEnumerable<SpaceHall>> GetAll();
        Space GetById(int id);
        void DeleteById(int id);
        Space UpdateById(int id , SpaceHall SpaceHall);
        void Add(SpaceHall SpaceHall);
    }
}
