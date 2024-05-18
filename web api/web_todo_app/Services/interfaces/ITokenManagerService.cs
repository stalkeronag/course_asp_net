using WebApi.Models;

namespace WebApi.Services.Interfaces
{
    public interface ITokenManagerService
    {
        public void SetRefreshToken(RefreshToken refreshToken);

        public void SetAccessToken(string accessToken);

        public string GetRefreshToken();

        public string GetAccessToken();

        public void RemoveRefreshToken();
    }
}
