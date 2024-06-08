using Microsoft.EntityFrameworkCore;
using Npgsql;
using web_todo_app.Models;
using WebApi.Data;
using WebApi.Models;

namespace web_todo_app.Extensions
{
    public static class DbExtensions
    {
        public static void ConfigureEntityFramework(this IServiceCollection services, IConfiguration configuration)
        {

            var connectionStringBuilder = new NpgsqlConnectionStringBuilder(configuration["ConnectionStrings:PostgreSql"]);
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(connectionStringBuilder.ConnectionString);
            });

            services.AddIdentity<User, UserRole>().AddEntityFrameworkStores<AppDbContext>();
        }
    }
}
