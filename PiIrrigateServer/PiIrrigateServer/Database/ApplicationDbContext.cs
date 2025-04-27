using Microsoft.EntityFrameworkCore;
using PiIrrigateServer.Models;
using System.Reflection.Metadata;

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
            modelBuilder.Entity<Zone>()
                .HasMany(z => z.Devices)
                .WithOne(d => d.Zone)
                .HasForeignKey(d => d.ZoneId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Device>()
                .HasOne(d => d.Zone)
                .WithMany(z => z.Devices)
                .HasForeignKey(d => d.ZoneId);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Zone> Zones { get; set; }
    }
}
