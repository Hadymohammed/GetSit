using GetSit.Models;

namespace GetSit.Data.Services
{
    public interface ISpaceService
    {
        Task<IEnumerable<Space>> GetAll();
        Space GetById(int id);
        void DeleteById(int id);
        Space UpdateById(int id , Space space);
        void Add(Space space);
    }
}
