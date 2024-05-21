using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace TourPlannerDAL
{
    public class TourRepository
    {
        private readonly TourPlannerDbContext _dbContext;

        public TourRepository(TourPlannerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddTourAsync(Tour tour){
            _dbContext.Tours.Add(tour);
            await _dbContext.SaveChangesAsync();
            await _dbContext.Entry(tour).GetDatabaseValuesAsync();
        }


        public async Task UpdateTourAsync(Tour tour){
            _dbContext.Tours.Update(tour);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteTourAsync(Tour tour){
            var tourLogs = await _dbContext.TourLogs.Where(tl => tl.TourId == tour.Id).ToListAsync();
            _dbContext.TourLogs.RemoveRange(tourLogs);
            _dbContext.Tours.Remove(tour);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Tour>> GetToursAsync(){
            return await _dbContext.Tours.Include(t => t.TourLogs).ToListAsync();
        }
    }
}