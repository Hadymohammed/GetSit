using GetSit.Data.Base;
using GetSit.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;

namespace GetSit.Data.Services
{
    public class FavoriteHallService : EntityBaseRepository<FavoriteHall>, IFavoriteHallService
    {
        private readonly AppDBcontext _context;
        public FavoriteHallService(AppDBcontext context):base(context)
        {
            _context = context;
        }
        public FavoriteHall GetByHallIdAndUserId(int HId, int CId)
        {
            var favoriteHall= _context.FavoriteHall.Where(h => h.HallId == HId).Where(c => c.CustomerId == CId).FirstOrDefault();
            return favoriteHall;
        }

        public List<FavoriteHall> GetByUserId(int CustomerId)
        {
            return _context.FavoriteHall.Where(c => c.CustomerId == CustomerId)
                    .Include(h => h.SpaceHall)
                        .ThenInclude(p => p.HallPhotos)
                    .Include(c => c.SpaceHall)
                        .ThenInclude(x => x.Space).ToList();    
        }

        public async Task ToggleAysnc(int HId,int CId)
        {
            FavoriteHall FavHall = GetByHallIdAndUserId(HId,CId);
            if (FavHall != null)
            {
                await DeleteAsync(FavHall.Id);
            }
            else
            {
                await AddAsync(new FavoriteHall()
                {
                    CustomerId = CId,
                    HallId = HId
                });
            }
            _context.SaveChanges();
        }
    }
}
