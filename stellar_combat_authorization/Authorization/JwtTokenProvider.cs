using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using StellarCombatAuthorization.Interfaces;

namespace StellarCombatAuthorization.Authorization
{
    public class JwtTokenProvider : ITokenProvider, ITokenValidator
    {
        private readonly string _secret;
        private readonly string _issuer;

        public JwtTokenProvider(string secret, string issuer)
        {
            _secret = secret;
            _issuer = issuer;
        }
        
        public string Provide(Guid playerId, uint sessionId)
        {
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("PlayerId", playerId.ToString()),
                    new Claim("SessionId", sessionId.ToString())
                }),
                
                Issuer = _issuer,
                SigningCredentials = GetSigningCredentials(GetSecurityKey())
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(descriptor);

            return handler.WriteToken(token);
        }

        public bool Validate(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            try
            {
                handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    IssuerSigningKey = GetSecurityKey(),
                    ValidIssuer = _issuer,
                }, out _);
            }
            catch
            {
                return false;
            }

            return true;
        }

        private SymmetricSecurityKey GetSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        }

        private SigningCredentials GetSigningCredentials(SecurityKey securityKey)
        {
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);
        }
    }
}

