using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web_todo_app.Dto;
using web_todo_app.Exceptions;
using web_todo_app.Services.interfaces;
using WebApi.Exceptions;
using WebApi.Models;
using WebApi.Services.Interfaces;

namespace web_todo_app.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private IUserService userService;

        private ILogger<AuthController> logger;

        private IMapper mapper;

        private ITokenService tokenService;

        private IUserRoleService roleService;

        private IAuthService authService;

        private IRefreshTokenSessionService refreshTokenSessionService;

        private IFingerprintService fingerprintService;

        private IRefreshTokenSessionBuilderService refreshTokenSessionBuilderService;

        private ITokenManagerService tokenManagerService;

        private IRefreshTokenSessionConnectionBuilder connectionBuilderService;

        private IUserCacheService userCacheService;
        public AuthController(IUserService userService,
            ITokenService tokenService,
            IUserRoleService roleService,
            IMapper mapper,
            ILogger<AuthController> logger,
            IAuthService authService,
            IRefreshTokenSessionService refreshTokenSessionService,
            IFingerprintService fingerprintService,
            IRefreshTokenSessionBuilderService refreshTokenSessionBuilderService,
            ITokenManagerService tokenManagerService,
            IRefreshTokenSessionConnectionBuilder tokenSessionConnectionBuilder,
            IUserCacheService userCacheService)
        {
            this.userService = userService;
            this.tokenService = tokenService;
            this.roleService = roleService;
            this.mapper = mapper;
            this.authService = authService;
            this.refreshTokenSessionService = refreshTokenSessionService;
            this.fingerprintService = fingerprintService;
            this.refreshTokenSessionBuilderService = refreshTokenSessionBuilderService;
            this.tokenManagerService = tokenManagerService;
            this.connectionBuilderService = tokenSessionConnectionBuilder;
            this.logger = logger;
            this.userCacheService = userCacheService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
        {
            try
            {
                User currentUser = await userService.GetUserByLogin(loginDto);
                var role = roleService.GetRolesByUserId(currentUser.Id).First();
                string accessToken = tokenService.GenerateAccessToken(currentUser, role);
                var refreshToken = tokenService.GenerateRefreshToken();
                var refreshSession = await refreshTokenSessionService.GetExistSessionOrCreate(currentUser);
                await refreshTokenSessionBuilderService
                    .AddRefreshTokenConnection(connectionBuilderService
                    .AddRefreshToken(refreshToken)
                    .AddFingerPrint(fingerprintService.GetFingerPrint(currentUser.Id))
                    .Build())
                    .Build(refreshSession);
                tokenManagerService.SetRefreshToken(refreshToken);
                tokenManagerService.SetAccessToken(accessToken);
                logger.LogInformation("login successfull");
                await userCacheService.CacheUser(currentUser);
                return Ok();
            }
            catch (UserNotFoundException userNotFound)
            {
                logger.LogError(userNotFound.Message);
                return BadRequest("user not found");
            }
            catch(GmailNotExistException gmailNotExist)
            {
                logger.LogError(gmailNotExist.Message);
                return BadRequest("gmail not found");
            }
            catch (WrongPasswordUserException wrongPassword)
            {
                logger.LogInformation("user by email {Email} input incorrect password", loginDto.Email);
                logger.LogError(wrongPassword.Message);
                return BadRequest("password incorrect");
            }
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
        {

            try
            {
                var registerUser = await userService.GetUserByEmail(registerDto.Email);
                logger.LogInformation("attempt register user with exist email {Email}", registerDto.Email);
                return BadRequest("such mail exist");
            }
            catch (UserNotFoundException userNotFound)
            {
                logger.LogInformation(userNotFound.Message + "continue register user by email {Email}", registerDto.Email);
                var user = mapper.Map<User>(registerDto);
                await userCacheService.CacheUser(user);
                await userService.AddUser(user, registerDto.Password);
                logger.LogInformation("registerUser by email {Email}", registerDto.Email);
                return Ok();   
            }
        }


        [HttpGet("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var refreshToken = tokenService.GetRefreshToken();
                tokenManagerService.RemoveRefreshToken();
                var refreshSession = refreshTokenSessionService.GetSessionByRefreshToken(refreshToken.Token);
                
                var currentUser = refreshSession.User;
                RefreshToken newRefreshToken = tokenService.GenerateRefreshToken();
                string newAccessToken = tokenService.GenerateAccessToken(currentUser,
                    roleService.GetRolesByUserId(currentUser.Id).First());
                await refreshTokenSessionBuilderService
                    .RemoveRefreshTokenConnection(connectionBuilderService
                    .AddRefreshToken(refreshToken)
                    .Build())
                    .AddRefreshTokenConnection(connectionBuilderService
                    .AddRefreshToken(newRefreshToken)
                    .AddFingerPrint(fingerprintService.GetFingerPrint(currentUser.Id))
                    .Build())
                    .Build(refreshSession);
                tokenManagerService.SetRefreshToken(newRefreshToken);
                tokenManagerService.SetAccessToken(newAccessToken);
                logger.LogInformation("Successfully refresh tokens");

            }
            catch (SessionNotExistException sessionException)
            {
                logger.LogError(sessionException.Message);
                logger.LogInformation(DateTime.UtcNow.ToString() + ": " +
                    "not valid refresh token" + ": ");
                return BadRequest("something went wrong");
            }
            catch (RefreshSessionBuilderException builderException)
            {
                logger.LogError(builderException.Message);
                return BadRequest("something went wrong");
            }
            catch (UserNotFoundException userNotFoundException)
            {
                logger.LogError(userNotFoundException.Message);
                logger.LogInformation("not successfull try refresh token");
                return BadRequest("users not found");
            }
            return Ok();
        }

        [Authorize]
        [HttpGet("LogOut")]
        public async Task<IActionResult> LogOut()
        {
            try
            {
                var refreshToken = tokenService.GetRefreshToken();
                var refreshSession = refreshTokenSessionService.GetSessionByRefreshToken(refreshToken.Token);
                tokenManagerService.RemoveRefreshToken();

                await refreshTokenSessionBuilderService
                        .RemoveRefreshTokenConnection(connectionBuilderService
                        .AddRefreshToken(refreshToken)
                        .Build())
                        .Build(refreshSession);
                logger.LogInformation("successfully log out");
            }
            catch (SessionNotExistException sessionException)
            {
                logger.LogError(sessionException.Message);
                logger.LogInformation(DateTime.UtcNow.ToString() + ": " +
                    "not valid refresh token" + ": ");
                return BadRequest("something went wrong");
            }
            catch (RefreshSessionBuilderException builderException)
            {
                logger.LogError(builderException.Message);
                return BadRequest("something went wrong");
            }  
            return Ok();
        }
    }
}
