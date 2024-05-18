using System.Security.Cryptography;
using System.Text;
using WebApi.Data;
using WebApi.Models;
using WebApi.Services.Interfaces;

namespace WebApi.Services.Implementations
{
    public class FingerPrintService : IFingerprintService
    {
        private readonly IHttpContextAccessor contextAccessor;

        private readonly AppDbContext context;

        public FingerPrintService(IHttpContextAccessor contextAccessor, AppDbContext context)
        {
            this.contextAccessor = contextAccessor;
            this.context = context;
        }

        public bool FindFingerPrint(string hashFingerPrint)
        {
            var fingerPrint = context.FingerPrints.Where(fingerPrint => fingerPrint.Hash.Equals(hashFingerPrint)).FirstOrDefault();

            if (fingerPrint != null)
                return true;
            else
                return false;
        }

        public FingerPrint GetFingerPrint(string userId)
        {
            string referer = contextAccessor.HttpContext.Request.Headers["Referer"];
            string userAgent = contextAccessor.HttpContext.Request.Headers["User-Agent"];
            FingerPrint fingerPrint = new FingerPrint()
            {
                Referer = referer,
                UserAgent = userAgent,
            };
            
            string hash = HashFingerPrint(fingerPrint, userId);

            fingerPrint.Hash = hash;
            return fingerPrint;
        }


        private string HashFingerPrint(FingerPrint fingerPrint, string userId)
        {
            var bytes = Encoding.UTF8.GetBytes(fingerPrint.UserAgent + fingerPrint.Referer + userId);
            byte[] result;
            using (var sha512 = SHA512.Create())
            {
                result = sha512.ComputeHash(bytes);
            }

            return BitConverter.ToString(result).Replace("-", String.Empty);
        }
    }
}
