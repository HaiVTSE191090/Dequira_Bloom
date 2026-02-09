using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IPasswordHasher
    {
        /// <summary>
        /// Hash a password using Argon2
        /// </summary>
        string HashPassword(string password);

        /// <summary>
        /// Verify a password against a hash
        /// </summary>
        bool VerifyPassword(string password, string hash);
    }
}
