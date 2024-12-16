using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using WebApplication2.Token;

namespace WebApplication2.Token
{
    public class TokenAuthenticationService : IAuthenticateServices
    {

        private readonly TokenManagement _tokenManagement;
        public TokenAuthenticationService(IOptions<TokenManagement> tokenManagement)
        {

            _tokenManagement = tokenManagement.Value;
        }
        public bool IsAuthenticated(Model.USERS request, out string token)
        {
            token = string.Empty;

            var claims = new[]
            {
                 new Claim(ClaimTypes.Name,request.USERNAME),
                 new Claim("password",request.PASSWORD.ToString()),
                 new Claim("name",request.USERNAME)
             };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(_tokenManagement.Issuer, _tokenManagement.Audience, claims, expires: DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration), signingCredentials: credentials);

            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return true;
            

        }
    }
}
