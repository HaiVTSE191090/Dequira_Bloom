using Application.Common.Interfaces;
using Konscious.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            var salt = CreateSalt();
            var hash = HashPasswordWithSalt(password, salt);
            return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }

        public bool VerifyPassword(string password, string hash)
        {
            var parts = hash.Split(':');
            var salt = Convert.FromBase64String(parts[0]);
            var hashBytes = Convert.FromBase64String(parts[1]);
            var testHash = HashPasswordWithSalt(password, salt);
            return hashBytes.SequenceEqual(testHash);
        }

        private byte[] CreateSalt()
        {
            var buffer = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(buffer);
            return buffer;
        }

        private byte[] HashPasswordWithSalt(string password, byte[] salt)
        {
            using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = 8,
                Iterations = 4,
                MemorySize = 128 * 1024
            };
            return argon2.GetBytes(16);
        }
    }
}
