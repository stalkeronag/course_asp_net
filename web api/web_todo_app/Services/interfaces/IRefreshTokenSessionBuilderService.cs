using WebApi.Models;

namespace WebApi.Services.Interfaces
{
    public interface IRefreshTokenSessionBuilderService
    {
        public Task Build(RefreshTokenSession refreshTokenSession);

        public IRefreshTokenSessionBuilderService AddRefreshTokenConnection(RefreshTokenSessionConnection connection);

        public IRefreshTokenSessionBuilderService RemoveRefreshTokenConnection(RefreshTokenSessionConnection connection);
    }
}
