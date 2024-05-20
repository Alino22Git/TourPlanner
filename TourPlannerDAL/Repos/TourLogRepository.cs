using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace TourPlannerDAL
{
    public class TourLogRepository
    {
        private readonly TourPlannerDbContext _dbContext;

        public TourLogRepository(TourPlannerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddTourLogAsync(TourLog tourLog)
        {
            try
            {
                _dbContext.TourLogs.Add(tourLog);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
        }

        public async Task<List<TourLog>> GetTourLogsAsync()
        {
            return await _dbContext.TourLogs.ToListAsync();
        }

        public async Task<List<TourLog>> GetTourLogsByTourIdAsync(int tourId)
        {
            return await _dbContext.TourLogs
                .Where(tl => tl.TourId == tourId)
                .ToListAsync();
        }

        public async Task UpdateTourLogAsync(TourLog tourLogToUpdate)
        {
            try
            {
                _dbContext.TourLogs.Update(tourLogToUpdate);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
        }

        public async Task DeleteTourLogAsync(TourLog tourLog)
        {
            try
            {
                _dbContext.TourLogs.Remove(tourLog);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
        }
    }
}