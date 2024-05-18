using WebApi.Data;
using WebApi.Models;
using WebApi.Services.Interfaces;

namespace WebApi.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext context;

        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task<User> GetCurrentUser()
        {
            var claims = httpContextAccessor.HttpContext.User.Claims;
            var userId = claims.First(claim => claim.Type.Equals("user_id")).Value;
            User currentUser = context.Users.Where(user => user.Id == userId).FirstOrDefault();
            return Task.FromResult(currentUser);
        }

        public Task SignIn()
        {
            throw new NotImplementedException();
        }

        public Task SignOut()
        {
            throw new NotImplementedException();
        }
    }
}
