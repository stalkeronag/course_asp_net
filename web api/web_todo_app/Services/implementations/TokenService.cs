using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using web_todo_app.Models;
using WebApi.Data;
using WebApi.Models;
using WebApi.Services.Interfaces;

namespace WebApi.Services.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;

        private readonly IHttpContextAccessor httpContextAccessor;

        private readonly AppDbContext context;

        public TokenService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, AppDbContext context)
        {
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
            this.context = context;
        }

        public string GenerateAccessToken(User user, UserRole role)
        {
            var date = DateTime.Now;
            var userClaims = new List<Claim>();
            userClaims.Add(new Claim("user_id", user.Id));
            userClaims.Add(new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Name, user.UserName));
            userClaims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name));
            var jwt = new JwtSecurityToken(
                issuer: configuration["Jwt:ValidIssuer"],
                audience: configuration["Jwt:ValidAudience"],
                claims: userClaims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(10)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])), SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public RefreshToken GenerateRefreshToken()
        {
            var ticks = DateTime.Now.Ticks;
            var guid = Guid.NewGuid().ToString();
            return new RefreshToken()
            {
                Token = guid + ticks,
                Expires = DateTime.UtcNow.AddDays(int.Parse(configuration["RefreshToken:Lifetime"])),
                Created = DateTime.UtcNow
            };
        }

        public string GetAccessToken()
        {
            return httpContextAccessor.HttpContext.Request.Headers["Authorization:Bearer"];
        }

        public RefreshToken GetRefreshToken()
        {
            string refreshToken =  httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            return context.RefreshTokens.Where(rt => rt.Token.Equals(refreshToken)).FirstOrDefault();
        }
    }
}
