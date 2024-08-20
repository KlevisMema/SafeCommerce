using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace SafeCommerce.Security.JwtSecurity.Helpers;

public class Helper_JwtToken
{
    public static string?
    GetUserIdDirectlyFromJwtToken
    (
        string token
    )
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        if (jwtToken == null)
            return null;

        var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
        return userIdClaim?.Value;
    }
}