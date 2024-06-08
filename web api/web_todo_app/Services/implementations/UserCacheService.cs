using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;
using web_todo_app.Services.interfaces;
using WebApi.Models;

namespace web_todo_app.Services.implementations
{
    public class UserCacheService : IUserCacheService
    {
        private readonly IConfiguration configuration;

        private readonly IDistributedCache cache;

        public UserCacheService(IConfiguration configuration, IDistributedCache cache)
        {
            this.configuration = configuration;
            this.cache = cache;
        }


        public async Task CacheUser(User user)
        {
            string userString = JsonConvert.SerializeObject(user);
            var cacheOptions = new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = new DateTimeOffset(DateTime.UtcNow.AddMinutes(int.Parse(configuration["TTLUserEmailKey"])))
            };
            await cache.SetStringAsync(user.Email, userString, cacheOptions);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var bytes = await cache.GetAsync(email);

            return JsonConvert.DeserializeObject<User>(Encoding.UTF8.GetString(bytes));
        }
    }
}
