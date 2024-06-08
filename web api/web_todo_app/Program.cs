using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using Serilog;
using Serilog.Sinks.OpenSearch;
using System.Security.Claims;
using System.Text;
using web_todo_app.Data;
using web_todo_app.Extensions;
using web_todo_app.Services.implementations;
using web_todo_app.Services.interfaces;
using WebApi.Data;
using WebApi.Mapping;
using WebApi.Models;
using WebApi.Services.Implementations;
using WebApi.Services.Interfaces;
var builder = WebApplication.CreateBuilder(args);


var logger = new LoggerConfiguration()
.WriteTo.OpenSearch(new OpenSearchSinkOptions(new Uri(builder.Configuration["ConnectionStrings:OpenSearch"]))
{
    IndexFormat = "custom-index-{0:yyyy.MM}"
}).CreateLogger();
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
    var policy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().RequireClaim(ClaimsIdentity.DefaultRoleClaimType,"user","admin").Build();
    options.DefaultPolicy = policy;
    options.AddPolicy("IsAdmin", policy => policy.RequireClaim(ClaimsIdentity.DefaultRoleClaimType, "admin").AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)); 
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
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["ConnectionStrings:redis"];
    options.InstanceName = builder.Configuration["redis:InstanceName"];
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
builder.Services.AddTransient<IFileCacheService, FileCacheService>();
builder.Services.AddTransient<IUserCacheService, UserCacheService>();
builder.Services.AddScoped<IRabbitMqService, RabbitMqService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.ConfigureEntityFramework(builder.Configuration);
builder.Services.AddAutoMapper(typeof(UserProfile));
builder.Logging.ClearProviders();
builder.Services.AddSerilog(logger);
builder.Services.AddTransient<IFileService, FileService>();
var app = builder.Build();
var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
var roleService = scope.ServiceProvider.GetRequiredService<IUserRoleService>();
var seeder = new Seeder(userService, roleService);
await seeder.Seed(context);
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();



app.MapControllers();

app.Run();


