using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

namespace SafeCommerce.Security.User.Interfaces;

public class Argon2PasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : class
{
    private readonly int _saltSize = 16; // 128-bit salt
    private readonly int _hashSize = 32; // 256-bit hash
    private readonly int _iterations = 6; // 6 iterations for higher security
    private readonly int _memorySize = 65536; // 64 MiB of memory (64 * 1024 KiB)
    private readonly int _degreeOfParallelism = 4; // 4 threads for parallelism

    public string 
    HashPassword
    (
        TUser user, 
        string password
    )
    {
        byte[] salt = new byte[_saltSize];
        RandomNumberGenerator.Fill(salt);

        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = _degreeOfParallelism,
            Iterations = _iterations,
            MemorySize = _memorySize
        };

        byte[] hash = argon2.GetBytes(_hashSize);

        byte[] hashBytes = new byte[_saltSize + _hashSize];
        Array.Copy(salt, 0, hashBytes, 0, _saltSize);
        Array.Copy(hash, 0, hashBytes, _saltSize, _hashSize);

        return Convert.ToBase64String(hashBytes);
    }

    public PasswordVerificationResult 
    VerifyHashedPassword
    (
        TUser user, 
        string hashedPassword, 
        string providedPassword
    )
    {
        byte[] hashBytes = Convert.FromBase64String(hashedPassword);

        byte[] salt = new byte[_saltSize];
        Array.Copy(hashBytes, 0, salt, 0, _saltSize);

        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(providedPassword))
        {
            Salt = salt,
            DegreeOfParallelism = _degreeOfParallelism,
            Iterations = _iterations,
            MemorySize = _memorySize
        };

        byte[] hash = argon2.GetBytes(_hashSize);

        for (int i = 0; i < _hashSize; i++)
        {
            if (hashBytes[i + _saltSize] != hash[i])
            {
                return PasswordVerificationResult.Failed;
            }
        }

        return PasswordVerificationResult.Success;
    }
}
