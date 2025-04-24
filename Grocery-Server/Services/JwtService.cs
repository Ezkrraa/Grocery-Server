using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Grocery_Server.Services
{
    public class JwtService
    {
        public string GenerateToken(Models.User user, IConfiguration configuration)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            DateTime expires = DateTime.Now.AddDays(Convert.ToDouble(configuration["Jwt:ExpireDays"]));

            JwtSecurityToken token = new JwtSecurityToken(
                    configuration["Jwt:Issuer"],
                    configuration["Jwt:Issuer"],
                    claims,
                    expires: expires,
                    signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
