using Microsoft.EntityFrameworkCore;
using web_todo_app.Models;

namespace web_todo_app.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<TaskModel> Tasks { get; set; }


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
