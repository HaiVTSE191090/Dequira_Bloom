using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IJwtTokenService
    {
        /// <summary>
        /// Generate access token for authenticated user
        /// </summary>
        string GenerateAccessToken(User user);

        /// <summary>
        /// Generate refresh token
        /// </summary>
        string GenerateRefreshToken();

        /// <summary>
        /// Get user ID from token
        /// </summary>
        Guid? GetUserIdFromToken(string token);

        /// <summary>
        /// Validate token
        /// </summary>
        bool ValidateToken(string token);
    }
}
