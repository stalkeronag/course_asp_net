using WebApi.Models;

namespace WebApi.Services.Interfaces
{
    public interface IRefreshTokenSessionService
    {
        public Task<RefreshTokenSession> GetExistSessionOrCreate(User user);

        public Task DeleteSessionById(string id);

        public bool SessionExist(User user);

        public RefreshTokenSession GetSessionByRefreshToken(string refreshToken);

    }
}
