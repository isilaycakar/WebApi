using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace _04_JwtAuth
{
    public class TokenService
    {
        public const string secretKey = "6331AC743C774BEA947CFA72B9417C73";
        public const string issuer = "api.com";
        public const string audience = "api.com";
        public static string GenerateToken(string username)
        {
            byte[] key = Encoding.UTF8.GetBytes(secretKey);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, "99"),
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "admin")
                };

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(issuer, audience, claims, null, DateTime.Now.AddDays(3), signingCredentials);

            string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return token;
        }
    }
}
