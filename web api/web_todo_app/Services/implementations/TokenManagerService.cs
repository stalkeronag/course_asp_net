using WebApi.Models;
using WebApi.Services.Interfaces;

namespace WebApi.Services.Implementations
{
    public class TokenManagerService : ITokenManagerService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public TokenManagerService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GetAccessToken()
        {
            return httpContextAccessor.HttpContext.Request.Headers["Authorization:Bearer"];
        }

        public string GetRefreshToken()
        {

            return httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
        }

        public void RemoveRefreshToken()
        {
            httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken");
        }

        public void SetAccessToken(string accessToken)
        {
            httpContextAccessor.HttpContext.Response.Headers.Append("token", accessToken);
        }

        public void SetRefreshToken(RefreshToken refreshToken)
        {
            CookieOptions cookieOptions = new CookieOptions()
            {
                Expires = refreshToken.Expires,
                HttpOnly = true,
            };

            httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }
    }
}
