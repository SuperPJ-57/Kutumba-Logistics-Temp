﻿using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.Persistence.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtTokenSetting _jwtSettings;
        public TokenService(IOptions<JwtTokenSetting> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }
        /// <summary>
        /// Generate token
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public string GenerateToken(List<Claim> claims)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            TimeSpan tokenlifetime = TimeSpan.FromHours(10);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(tokenlifetime),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = signingCredentials
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


    }
}
