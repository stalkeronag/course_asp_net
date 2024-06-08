using WebApi.Models;

namespace web_todo_app.Services.interfaces
{
    public interface IUserCacheService
    {
        public Task CacheUser(User user);

        public Task<User> GetUserByEmail(string email);
    }
}
