using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;

namespace Ping.Commons.Settings
{
    public static class TokenService
    {
        // Generates a JWT Token using a client identifier string (app-end-user id. or microservice id.) and signing it with a secret key
        //  --- The client identifier string is used to generate a Claim of type Claim.Name
        //  --- The secret signing key is later used for token decrypt.
        public static string GenerateToken(string clientIdentifier, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, clientIdentifier)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // sign using hmac
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
