using APi.Dtos;
using APi.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APi.Interfaces
{
    public interface ILIkeRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int likeUserid);
        Task<AppUser> GetUserWithLike(int userId);
        Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId);
        
    }
}
