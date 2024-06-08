using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using web_todo_app.Services.interfaces;
using WebApi.Services.Interfaces;

namespace web_todo_app.Controllers
{
    
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService userService;

        private readonly IDistributedCache cache;

        private readonly IUserCacheService userCacheService;

        public UserController(IUserService userService, IDistributedCache cache, IUserCacheService userCacheService)
        {
            this.userService = userService;
            this.cache = cache;
            this.userCacheService = userCacheService;
        }

        
        [HttpGet("GetAllUser")]
        [Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await userService.GetAllUsersAsync();
            return Ok(users);
        }

        
        [HttpGet("GetUserByEmail")]
        [Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var userWithCache = await userCacheService.GetUserByEmail(email);

            if (userCacheService != null)
            {
                return Ok(userWithCache);
            }

            var userWithDB = await userService.GetUserByEmail(email);

            if (userWithDB != null)
            {
                return Ok(userWithDB);
            }

            return Ok();
        }

        [HttpPost("DeleteUser")]
        [Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> DeleteUserById(string userId)
        {
            await userService.DeleteUserById(userId);
            return Ok();
        }
    }
}
