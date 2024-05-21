using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace TourPlannerDAL
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TourPlannerDbContext>
    {
        public TourPlannerDbContext CreateDbContext(string[] args){
            var optionsBuilder = new DbContextOptionsBuilder<TourPlannerDbContext>();
            var connectionString = "Host=localhost;Port=5432;Database=postgres;Username=root;Password=tourplanner";
            optionsBuilder.UseNpgsql(connectionString);
            return new TourPlannerDbContext(optionsBuilder.Options);
        }
    }
}