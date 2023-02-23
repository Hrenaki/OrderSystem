using Microsoft.IdentityModel.Tokens;
using OrderSystem.API.Settings;
using OrderSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Core.Services
{
    public class UserAuthenticationService
    {
        private readonly IUserService userService;
        private readonly HashService hashService;
        private readonly JwtBearerSettings jwtBearerSettings;

        public UserAuthenticationService(IUserService userService, HashService hashService, JwtBearerSettings jwtBearerSettings)
        {
            this.userService = userService;
            this.jwtBearerSettings = jwtBearerSettings;
            this.hashService = hashService;
        }

        public AuthenticationResult Login(string username, string password)
        {
            var user = userService.GetUserByUsername(username);
            if (user is null)
                return new AuthenticationResult() { Error = $"User '{username}' not found" };

            var actualPasswordHash = hashService.GetHash(password);
            if (actualPasswordHash != user!.PasswordHash)
                return new AuthenticationResult() { Error = $"Password is wrong" };

            (var jwtToken, var expiresAt) = CreateJwtToken(username, user.Role);
            return new AuthenticationResult() { AccessToken = jwtToken, ExpiresAt = expiresAt };
        }

        public AuthenticationResult Register(string username, string password, UserRole role)
        {
            var user = userService.GetUserByUsername(username);
            if (user is not null)
                return new AuthenticationResult() { Error = $"User '{username}' not found" };

            var passwordHash = hashService.GetHash(password);
            var userEntity = new UserEntity() { Username = username, PasswordHash = passwordHash, Role = role };
            var result = userService.CreateUser(userEntity);
            if (!result.Success)
                return new AuthenticationResult() { Error = result.Message };

            (var jwtToken, var expiresAt) = CreateJwtToken(userEntity.Username, userEntity.Role);
            return new AuthenticationResult() { AccessToken = jwtToken, ExpiresAt = expiresAt };
        }

        private (string Token, DateTime ExpiresAt) CreateJwtToken(string username, UserRole role)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role.ToString())
            };

            var expiresAt = DateTime.Now.AddYears(10);
            var jwtToken = new JwtSecurityToken(
                issuer: jwtBearerSettings.Issuer,
                claims: claims,
                expires: expiresAt,
                signingCredentials: new SigningCredentials(jwtBearerSettings.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );

            return (new JwtSecurityTokenHandler().WriteToken(jwtToken), expiresAt);
        }
    }

    public class AuthenticationResult
    {
        public string AccessToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string Error { get; set; }
    }
}