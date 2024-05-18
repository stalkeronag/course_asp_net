using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using web_todo_app.Models;
using WebApi.Models;

namespace WebApi.Data
{
    public class AppDbContext : IdentityDbContext<User, UserRole, string>
    {
        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<RefreshTokenSession> RefreshTokenSessions { get; set; }

        public DbSet<RefreshTokenSessionConnection> RefreshTokenSessionConnections  { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<FingerPrint> FingerPrints { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            ApplyMigrations(this);
        }

        public void ApplyMigrations(AppDbContext context)
        {
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }
    }
}
