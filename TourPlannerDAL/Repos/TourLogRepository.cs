using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;
using TourPlannerLogging;
namespace TourPlannerDAL
{
    public class TourLogRepository
    {
        private readonly TourPlannerDbContext _dbContext;
        private static readonly ILoggerWrapper logger = LoggerFactory.GetLogger();

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
            catch (Exception e)
            {
                logger.Error($"Error adding tour log {e.Message}");
            }
        }

        public async Task<List<TourLog>> GetTourLogsAsync()
        {
            try
            {
                return await _dbContext.TourLogs.ToListAsync();
            }
            catch (Exception e)
            {
                logger.Error($"Error adding tour log {e.Message}");
                return new List<TourLog>();
            }
        }

        public async Task<List<TourLog>> GetTourLogsByTourIdAsync(int tourId)
        {
            try
            {
                return await _dbContext.TourLogs
                    .Where(tl => tl.TourId == tourId)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                logger.Error($"Error adding tour log {e.Message}");
                return new List<TourLog>();
            }
        }

        public async Task UpdateTourLogAsync(TourLog tourLogToUpdate)
        {
            try
            {
                _dbContext.TourLogs.Update(tourLogToUpdate);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.Error($"Error adding tour log {e.Message}");
            }
        }

        public async Task DeleteTourLogAsync(TourLog tourLog)
        {
            try
            {
                _dbContext.TourLogs.Remove(tourLog);
                await _dbContext.SaveChangesAsync();
            }
            catch (System.Exception e)
            {
                logger.Error($"Error adding tour log {e.Message}");
            }
        }
    }
}