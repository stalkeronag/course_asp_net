using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using System.Text;
using web_todo_app.Extensions;
using WebApi.Mapping;
using WebApi.Services.Implementations;
using WebApi.Services.Interfaces;
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddControllersWithViews();
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
        };
    });
    builder.Services.AddAuthorization(options =>
    {
        var policy = new AuthorizationPolicyBuilder("Bearer").RequireAuthenticatedUser().Build();

        options.DefaultPolicy = policy;
    });
    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
        {

            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "JWT Authorization header using the Bearer scheme."
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
            },
            new string[] {}
        }
    });
    });
    builder.Services.AddTransient<IUserService, UserService>();
    builder.Services.AddTransient<ITokenService, TokenService>();
    builder.Services.AddTransient<IUserRoleService, UserRoleService>();
    builder.Services.AddTransient<IAuthService, AuthService>();
    builder.Services.AddTransient<IRefreshTokenSessionService, RefreshTokenSessionService>();
    builder.Services.AddTransient<IFingerprintService, FingerPrintService>();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddTransient<IRefreshTokenSessionBuilderService, RefreshTokenSessionBuilderService>();
    builder.Services.AddTransient<ITokenManagerService, TokenManagerService>();
    builder.Services.AddTransient<IRefreshTokenSessionConnectionBuilder, RefreshTokenSessionConnectionBuilder>();
    builder.Services.ConfigureEntityFramework(builder.Configuration);
    builder.Services.AddAutoMapper(typeof(UserProfile));

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseAuthentication();
    app.UseAuthorization();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
    app.UseStaticFiles();




    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}

