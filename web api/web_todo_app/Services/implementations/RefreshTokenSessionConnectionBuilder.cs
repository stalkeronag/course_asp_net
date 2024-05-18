using WebApi.Models;
using WebApi.Services.Interfaces;

namespace WebApi.Services.Implementations
{
    public class RefreshTokenSessionConnectionBuilder : IRefreshTokenSessionConnectionBuilder
    {
        private FingerPrint FingerPrint { get; set; }

        private RefreshToken RefreshToken { get; set; }

        private string IpAddress { get; set; }


        public IRefreshTokenSessionConnectionBuilder AddFingerPrint(FingerPrint fingerPrint)
        {
            this.FingerPrint = fingerPrint;
            return this;
        }

        public IRefreshTokenSessionConnectionBuilder AddIpAddress(string ipAddress)
        {
            IpAddress = ipAddress;
            return this;
        }

        public IRefreshTokenSessionConnectionBuilder AddRefreshToken(RefreshToken refreshToken)
        {
            RefreshToken = refreshToken;
            return this;
        }

        public RefreshTokenSessionConnection Build()
        {
            return new RefreshTokenSessionConnection()
            {
                IpAddress = IpAddress,
                RefreshToken = RefreshToken,
                FingerPrint = FingerPrint
            };
        }
    }
}
