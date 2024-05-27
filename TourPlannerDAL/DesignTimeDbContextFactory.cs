using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.IO;

namespace TourPlannerDAL
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TourPlannerDbContext>
    {
        public TourPlannerDbContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var config = builder.Build();
            var connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<TourPlannerDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new TourPlannerDbContext(optionsBuilder.Options);
        }
    }
}