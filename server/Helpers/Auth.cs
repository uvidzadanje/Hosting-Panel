using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using server.Models;

namespace server.Helpers
{
    public class Auth
    {
        private IConfiguration configuration;

        public Auth(IConfiguration configuration)
        {
            this.configuration  = configuration;
        }
        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.configuration["Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { 
                    new Claim("id", user.ID.ToString()),
                    new Claim("priority", user.Priority.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public Dictionary<string, int> ValidateJwtToken(string authHeader)
        {
            try
            {
                Dictionary<string, int> tokenValues = new Dictionary<string, int>();
                var token = authHeader.Split(" ")[1];

                if(String.IsNullOrEmpty(token)) throw new Exception("Token doesn't exist!");

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(this.configuration["Secret"]);
                
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;

                tokenValues.Add("id", Int32.Parse(jwtToken.Claims.First(x => x.Type == "id").Value));
                tokenValues.Add("priority", Int32.Parse(jwtToken.Claims.First(x => x.Type == "id").Value));

                return tokenValues;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}