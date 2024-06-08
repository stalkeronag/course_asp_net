using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using WebApi.Data;
using WebApi.Models;
using WebApi.Services.Interfaces;

namespace web_todo_app.Data
{
    public class Seeder
    {
        private readonly IUserService _userService;

        private readonly IUserRoleService _roleService;

        public Seeder(IUserService userService, IUserRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        public async Task Seed(AppDbContext context)
        {
            if (context.Users.Any())
            {
                return;
            }
            var seedUsers = JsonConvert.DeserializeObject<SeedUserInfo>(File.ReadAllText("Resources/SeedUser.json"));
            foreach (var user in seedUsers.users)
            {

                await _roleService.AddRole(user.Role);

                User currentUser = new User()
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                await _userService.AddUser(currentUser, user.Password, user.Role);
            }
        }
    }
}
