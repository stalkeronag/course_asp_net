using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using web_todo_app.Services.interfaces;

namespace web_todo_app.Services.implementations
{
    public class FileCacheService : IFileCacheService
    {
        private readonly IDistributedCache cache;

        private readonly ILogger<FileCacheService> logger;

        private readonly IConfiguration configuration;

        public FileCacheService(IDistributedCache cache, ILogger<FileCacheService> logger, IConfiguration configuration)
        {
            this.cache = cache;
            this.logger = logger;
            this.configuration = configuration;
        }

        public async Task CacheFileName(string fileName, string path)
        {
            try
            {
                var timeExpiration = new DateTimeOffset(DateTime.UtcNow.AddMinutes(int.Parse(configuration["TTLFileNameKey"])));
                await cache.SetStringAsync(fileName, path, new DistributedCacheEntryOptions() { AbsoluteExpiration = timeExpiration });
                logger.LogInformation("success cache filename");
            }
            catch (Exception e)
            {

                logger.LogError("not success cache filename");
            }
        }

        public async Task<string> FindFileInCache(string fileName)
        {
            var path = await cache.GetAsync(fileName);
            
            if (path == null)
            {
                logger.LogInformation("fileName in cache not found");
                return null;
            }
            return Encoding.UTF8.GetString(path);
        }
    }
}
