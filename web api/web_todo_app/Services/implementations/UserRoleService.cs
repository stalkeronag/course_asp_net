using Microsoft.AspNetCore.Identity;
using WebApi.Data;
using WebApi.Models;
using WebApi.Services.Interfaces;

namespace WebApi.Services.Implementations
{
    public class UserRoleService : IUserRoleService
    {
        private readonly AppDbContext context;

        private readonly RoleManager<UserRole> roleManager;

        public UserRoleService(AppDbContext context, RoleManager<UserRole> roleManager)
        {
            this.context = context;
            this.roleManager = roleManager;
            
        }

        public async void AddRole(string roleName)
        {
            await roleManager.CreateAsync(new UserRole()
            {
                Name = roleName
            });
            await context.SaveChangesAsync();
        }

        public IEnumerable<UserRole> GetRolesByUserId(string userId)
        {
            return context.UserRoles.Where(userRoles => userRoles.UserId == userId).
                Join(context.Roles, userRoles => userRoles.RoleId, roles => roles.Id,
                (userRoles, roles) => roles);
        }
    }
}
