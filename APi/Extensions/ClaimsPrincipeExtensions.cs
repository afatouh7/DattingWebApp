using System.Security.Claims;

namespace APi.Extensions
{
    public static class ClaimsPrincipeExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
           return user.FindFirst(ClaimTypes.Name)?.Value;
        }
        public static int GetUSerId(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString());
        }
    }
}
