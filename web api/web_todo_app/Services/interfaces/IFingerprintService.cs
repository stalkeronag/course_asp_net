using WebApi.Models;

namespace WebApi.Services.Interfaces
{
    public interface IFingerprintService
    {
        public FingerPrint GetFingerPrint(string userId);

        public bool FindFingerPrint(string hashFingerPrint);

    }
}
