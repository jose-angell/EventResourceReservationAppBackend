using EventResourceReservationApp.Application.Services;
using EventResourceReservationApp.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventResourceReservationApp.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        public TokenService(IConfiguration config)
        {
            _config = config;
        }
        public string CreateToken(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                // Claim estándar de Identity para el ID del usuario
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                // Claim estándar de Identity para el email
                new Claim(ClaimTypes.Email, user.Email)
            };
            // Añadir los roles como claims
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            // 1. Obtener la clave secreta y la configuración
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]));

            // 2. Crear las credenciales de la firma
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            double expirationMinutes = _config["JwtSettings:TokenExpirationInMinutes"] != null ? double.Parse(_config["JwtSettings:TokenExpirationInMinutes"]) : 60;
            // 3. Definir el token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                // Expiración basada en la configuración
                Expires = DateTime.UtcNow.AddMinutes(
                     expirationMinutes),
                // Valores de appsettings.json
                Issuer = _config["JwtSettings:Issuer"],
                Audience = _config["JwtSettings:Audience"],
                SigningCredentials = credentials
            };
            // 4. Escribir (crear) el token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
