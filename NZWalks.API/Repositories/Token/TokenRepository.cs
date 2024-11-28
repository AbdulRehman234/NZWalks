using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NZWalks.API.Interfaces.Token;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace NZWalks.API.Repositories.Token
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration _configuration;
        public TokenRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string CreateJWTToken(IdentityUser user, List<string> roles)
        {
            //First Of all create Claim 
            /* claims are key-value pairs that provide information about the user.
             They are part of the identity system used to represent and manage user-related data and permissions.
            Claims are often used for authorization, customizing user profiles, or storing additional user-related information.
            To provide detailed user information for authorization.
            */ //key is thisIsARendomKeyWhichMayContainIntegerLike0123OrNumbarValueAlphabats

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            //now add configration key which is appsetting.json get ket through configration
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                                              _configuration["Jwt:Audience"],
                                              claims,
                                              expires: DateTime.Now.AddMinutes(15),
                                              signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
