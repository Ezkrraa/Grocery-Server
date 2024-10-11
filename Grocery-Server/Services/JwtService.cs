using System.Security.Claims;

namespace Grocery_Server.Services
{
    public class JwtService
    {
        public string GenerateToken(Models.User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };
            return "";
        }
    }
}
