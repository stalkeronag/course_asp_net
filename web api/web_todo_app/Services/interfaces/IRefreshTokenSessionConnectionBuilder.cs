using WebApi.Models;

namespace WebApi.Services.Interfaces
{
    public interface IRefreshTokenSessionConnectionBuilder
    {
        public RefreshTokenSessionConnection Build();

        public IRefreshTokenSessionConnectionBuilder AddFingerPrint(FingerPrint fingerPrint);

        public IRefreshTokenSessionConnectionBuilder AddRefreshToken(RefreshToken refreshToken);

        public IRefreshTokenSessionConnectionBuilder AddIpAddress(string  ipAddress);
    }
}
