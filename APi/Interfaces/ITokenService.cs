using APi.Entities;
using System.Threading.Tasks;

namespace APi.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
