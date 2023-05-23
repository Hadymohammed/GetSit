using GetSit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography;

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
        public async Task<IEnumerable<SpaceHall>> GetBySearch(String Key)
        {
            var result = await _context.SpaceHall.Include(x => x.Space).Include(y => y.HallPhotos).Where(p => p.Space.Name.Contains(Key)).ToListAsync();
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
        public void Fav(int HId,int CId)
        {
            FavoriteHall FavHall = _context.FavoriteHall.Where(h => h.HallId == HId).Where(c => c.CustomerId == CId).FirstOrDefault();
            if (FavHall != null)
            {
                _context.FavoriteHall.Remove(FavHall);
                

            }
            else
            {
                var favHall = new FavoriteHall()
                {
                    CustomerId = CId,
                    HallId = HId
                };
                
                _context.FavoriteHall.Add(favHall);
                
            }
            _context.SaveChanges();

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
