using Microsoft.AspNetCore.Identity;

namespace NZWalks.API.Interfaces.Token
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
