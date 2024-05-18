using web_todo_app.Dto;
using WebApi.Models;

namespace WebApi.Services.Interfaces
{
    public interface IUserService
    {
        public Task<User> GetUserByLogin(LoginUserDto loginUserDto);

        public Task<User> GetUserByEmail(string email);

        public Task<IEnumerable<User>> GetAllUsersAsync();

        public Task<User> GetUserById(string id);

        public Task AddUser(User user, string password);

        public Task UpdateUserById(string id);

        public Task DeleteUserById(string id);

        public Task AddRoleInUserById(string id, string role = "user");

    }
}
