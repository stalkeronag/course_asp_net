using WebApi.Models;

namespace WebApi.Services.Interfaces
{
    public interface IUserRoleService
    {
        public Task AddRole(string roleName);

        public IEnumerable<UserRole> GetRolesByUserId(string userId);
    }
}
