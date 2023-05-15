using GetSit.Models;
using Microsoft.EntityFrameworkCore;

namespace GetSit.Data.Services
{
    public class SpaceServices : ISpaceService
    {
        public readonly AppDBcontext _context;
        public SpaceServices(AppDBcontext context)
        {
            _context = context; 
        }
        public void Add(Space space)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Space>> GetAll()
        {
            var result=await _context.Space.ToListAsync();
            return result;
        }

        public Space GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Space UpdateById(int id, Space space)
        {
            throw new NotImplementedException();
        }
    }
}
