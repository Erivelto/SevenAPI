using Microsoft.Extensions.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using PlataformaSeven.API.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace PlataformaSeven.API.Services
{
    public interface IJwtService
    {
        string GenerateToken(Usuario usuario);
        string GenerateRefreshToken();
        Task<ClaimsPrincipal?> GetPrincipalFromExpiredToken(string token);
    }

    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly SymmetricSecurityKey _signingKey;

        public JwtService(IOptions<JwtSettings> jwtSettings, SymmetricSecurityKey signingKey)
        {
            _jwtSettings = jwtSettings.Value;
            _signingKey = signingKey;
        }

        /// <summary>
        /// Gera um token JWT para o usuário
        /// </summary>
        public string GenerateToken(Usuario usuario)
        {
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("sub", usuario.Id.ToString()),
                    new Claim("unique_name", usuario.User),
                    new Claim("UserId", usuario.Id.ToString()),
                    new Claim("Tipo", usuario.Tipo),
                    new Claim(ClaimTypes.Role, usuario.Tipo),
                    new Claim("jti", Guid.NewGuid().ToString()),
                }),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
                SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256)
            };

            var handler = new JsonWebTokenHandler();
            return handler.CreateToken(descriptor);
        }

        /// <summary>
        /// Gera um refresh token aleatório
        /// </summary>
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// Obtém o principal de um token expirado (para refresh)
        /// </summary>
        public async Task<ClaimsPrincipal?> GetPrincipalFromExpiredToken(string token)
        {
            // Sanitize token: extract only the JWT (3 Base64url segments separated by dots)
            var match = Regex.Match(token.Trim(), @"^([A-Za-z0-9_-]+)\.([A-Za-z0-9_-]+)\.([A-Za-z0-9_-]+)");
            if (match.Success)
            {
                token = $"{match.Groups[1].Value}.{match.Groups[2].Value}.{match.Groups[3].Value}";
            }

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = _signingKey
            };

            var handler = new JsonWebTokenHandler();

            try
            {
                var result = await handler.ValidateTokenAsync(token, tokenValidationParameters);
                if (!result.IsValid)
                    return null;

                return new ClaimsPrincipal(result.ClaimsIdentity);
            }
            catch
            {
                return null;
            }
        }
    }
}

