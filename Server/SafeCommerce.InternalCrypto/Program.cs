using System.Security.Cryptography;

namespace SafeCommerce.InternalCrypto;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(GenerateHMACSHA256Key());
    }

    public static (string publicKey, string privateKey) GenerateRsaKeyPair()
    {
        using var rsa = new RSACryptoServiceProvider(2048);
        var publicKey = rsa.ToXmlString(false);
        var privateKey = rsa.ToXmlString(true);
        return (publicKey, privateKey);
    }

    //pair of keys user specific or application specific

    public static string GenerateHMACSHA256Key()
    {
#pragma warning disable SYSLIB0023 // Type or member is obsolete
        using (var rng = new RNGCryptoServiceProvider())
        {
            byte[] secretKey = new byte[32];
            rng.GetBytes(secretKey);
            return Convert.ToBase64String(secretKey);
        }
#pragma warning restore SYSLIB0023 // Type or member is obsolete
    }
}