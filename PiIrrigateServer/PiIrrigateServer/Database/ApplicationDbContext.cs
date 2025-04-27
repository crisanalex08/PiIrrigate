using Microsoft.EntityFrameworkCore;
using PiIrrigateServer.Models;

namespace PiIrrigateServer.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            DotNetEnv.Env.Load();
            var connectionString = DotNetEnv.Env.GetString("DB_CONNECTION_STRING");
            optionsBuilder.UseNpgsql(connectionString);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Device> Devices { get; set; }
    }
}
