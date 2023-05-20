using GetSit.Models;
using Microsoft.EntityFrameworkCore;

namespace GetSit.Data.Services
{
    public class SpaceHallServices : ISpaceHallService
    {
        public readonly AppDBcontext _context;
        public SpaceHallServices(AppDBcontext context)
        {
            _context = context; 
        }
        public async Task<IEnumerable<SpaceHall>> GetAll()
        {
            var result = await _context.SpaceHall.Include(x => x.Space).Include(y=>y.HallPhotos).ToListAsync();
            return result;
        }
        public void Add(SpaceHall SpaceHall)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

       

        public Space GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Space UpdateById(int id, SpaceHall SpaceHall)
        {
            throw new NotImplementedException();
        }
    }
}
