using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using PiIrrigateServer.Entities;

namespace PiIrrigateServer.Database
{
    public class ApplicationDbContext : DbContext
    {
        private readonly string connectionString;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Env.Load();
            optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("DATA_CONNECTION"));
        }

        public DbSet<DataEntity> DataEntities => Set<DataEntity>();
    }

    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            Env.Load();
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("DATA_CONNECTION"));

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
