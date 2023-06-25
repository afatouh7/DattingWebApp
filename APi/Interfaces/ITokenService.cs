using APi.Entities;

namespace APi.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
