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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure User -> Zone relationship
            modelBuilder.Entity<User>()
                .HasMany(u => u.Zones)
                .WithOne(z => z.User)
                .HasForeignKey(z => z.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Zone -> Device relationship
            modelBuilder.Entity<Zone>()
                .HasMany(z => z.Devices)
                .WithOne(d => d.Zone)
                .HasForeignKey(d => d.ZoneId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Zone> Zones { get; set; }
    }
}
