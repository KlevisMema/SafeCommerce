using System.Text;
using System.Security.Cryptography;

namespace SafeCommerce.Security.JwtSecurity.Helpers;

internal class Security_HMACSHA256Helper
{
    private readonly byte[] _secretKey;

    internal Security_HMACSHA256Helper
    (
        byte[] secretKey
    )
    {
        _secretKey = secretKey;
    }

    internal string 
    ComputeHash
    (
        string data
    )
    {
        using (var hmac = new HMACSHA256(_secretKey))
        {
            var dataBytes = Encoding.UTF8.GetBytes(data);
            var hashBytes = hmac.ComputeHash(dataBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}