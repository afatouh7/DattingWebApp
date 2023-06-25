using Api.Dtos;
using APi.Entities;
using APi.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUserNameAsync(string userName);
        Task<PagedLIst<MemeberDto>> GetMembersAsync(UserParams userParams);
        Task<MemeberDto> GetMemberAsync(string username);
    }
}
