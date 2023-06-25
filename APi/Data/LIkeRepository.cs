using APi.Dtos;
using APi.Entities;
using APi.Extensions;
using APi.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APi.Data
{
    public class LIkeRepository : ILIkeRepository
    {
        private readonly DataContext _context;

        public LIkeRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<UserLike> GetUserLike(int sourceUserId, int likeUserid)
        {
            return await _context.Likes.FindAsync(sourceUserId,likeUserid);
        }

        public async Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var likes=_context.Likes.AsQueryable();
            if (predicate == "liked")
            {
                likes = likes.Where(like => like.SorceUserId == userId);
                users = likes.Select(like => like.LikeUser);
            }
            if (predicate == "likedBy")
            {
                likes = likes.Where(like => like.LikeUserId == userId);
                users = likes.Select(like => like.SorceUser);
            }
            return await users.Select(user => new LikeDto
            {
                UserName = user.UserName,
                KnowAs = user.KnownAs,
                Age = user.DateOfBirth.CalaculateAge(),
                PhotoUrl=user.Photos.FirstOrDefault(p=>p.IsMain).Url,
                City = user.City,
                Id = user.Id,
            }).ToListAsync();
        }

        
        public async Task<AppUser> GetUserWithLike(int userId)
        {

            return await _context.Users.Include(x => x.LikedUser).FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}
