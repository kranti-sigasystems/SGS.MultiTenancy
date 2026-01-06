using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SGS.MultiTenancy.Infra.Repositery
{
    /// <summary>
    /// Generates JSON Web Tokens for authenticated users.
    /// </summary>
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtTokenGenerator"/> class.
        /// </summary>
        /// <param name="jwtOptions">JWT configuration options.</param>
        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        /// <summary>
        /// Generates a signed JWT token containing user, role, and permission claims.
        /// </summary>
        /// <param name="applicationUser">Authenticated user.</param>
        /// <param name="roles">Roles assigned to the user.</param>
        /// <param name="permissions">Permissions granted to the user.</param>
        /// <returns>Serialized JWT token string.</returns>
        public string GenerateToken(User applicationUser, IEnumerable<string> roles, IEnumerable<string> permissions)
        {
            List<Claim> claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, applicationUser.ID.ToString()),
                new("tenantId", applicationUser.TenantID.ToString()),
                new(JwtRegisteredClaimNames.Email, applicationUser.Email),
                new(JwtRegisteredClaimNames.Name,applicationUser.Name),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            
            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            foreach (string permission in permissions)
            {
                claims.Add(new Claim("permission", permission));
            }

            SymmetricSecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtOptions.Secret));

            SigningCredentials credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtOptions.ExpiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
