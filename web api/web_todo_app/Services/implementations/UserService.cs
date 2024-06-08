using Microsoft.AspNetCore.Identity;
using web_todo_app.Dto;
using web_todo_app.Exceptions;
using WebApi.Data;
using WebApi.Exceptions;
using WebApi.Models;
using WebApi.Services.Interfaces;

namespace WebApi.Services.Implementations
{
    public class UserService : IUserService
    {
        private AppDbContext context;

        private UserManager<User> userManager;


        public UserService(AppDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task AddRoleInUserById(string id, string role = "user")
        {
            User currentUser = context.Users.Where(user => user.Id.Equals(id)).FirstOrDefault();

            if (currentUser != null)
            {
                await userManager.AddToRoleAsync(currentUser, role);
                await context.SaveChangesAsync();
            } 
        }

        public async Task AddUser(User user, string password, string role = "user")
        {
            string hashPass = userManager.PasswordHasher.HashPassword(user, password);
            user.PasswordHash = hashPass;
            await userManager.CreateAsync(user);
            await context.SaveChangesAsync();
            var currentUser = await userManager.FindByEmailAsync(user.Email);
            await AddRoleInUserById(currentUser.Id, role);   
        }

        public async Task DeleteUserById(string id)
        {
            await userManager.DeleteAsync(context.Users.Where(user => user.Id.Equals(id)).First());
            await context.SaveChangesAsync();
        }

        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            IEnumerable<User> users = context.Users.AsEnumerable();

            return Task.FromResult(users);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            User user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new UserNotFoundException("such user not exist");
            }
            return user;
        }

        public Task<User> GetUserById(string id)
        {
            User user = context.Users.Where(user => user.Id.Equals(id)).First();
            return Task.FromResult<User>(user);
        }

        public async Task<User> GetUserByLogin(LoginUserDto loginUserDto)
        {
            var user = await GetUserByEmail(loginUserDto.Email);

            if (user == null)
            {
                throw new GmailNotExistException("account with this gmail not exist");
            } 
            else
            {    
                if (await userManager.CheckPasswordAsync(user, loginUserDto.Password))
                {
                    return user;
                }
                else
                {
                    throw new WrongPasswordUserException("password is wrong");
                }
            }
        }

        public Task UpdateUserById(string id)
        {
            throw new NotImplementedException();
        }

    }
}
