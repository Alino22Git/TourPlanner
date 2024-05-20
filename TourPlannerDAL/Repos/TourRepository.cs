using System.Threading.Tasks;
using Models;
using Microsoft.EntityFrameworkCore;

namespace TourPlannerDAL
{
    public class TourRepository
    {
        private readonly TourPlannerDbContext _dbContext;

        public TourRepository(TourPlannerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddTourAsync(Tour tour)
        {
            _dbContext.Tours.Add(tour);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Tour>> GetToursAsync()
        {
            return await _dbContext.Tours.ToListAsync();
        }
        // Weitere Methoden für CRUD-Operationen können hier hinzugefügt werden
    }
}