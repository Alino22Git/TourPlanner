using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;
using TourPlannerLogging;
using TourPlannerDAL.Exceptions;

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
                string errorMsg = $"Error adding tour log: {e.Message}";
                logger.Error(errorMsg);
                throw new TourLogRepositoryException(errorMsg, e);
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
                string errorMsg = $"Error retrieving tour logs: {e.Message}";
                logger.Error(errorMsg);
                throw new TourLogRepositoryException(errorMsg, e);
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
                string errorMsg = $"Error retrieving tour logs by tour ID: {e.Message}";
                logger.Error(errorMsg);
                throw new TourLogRepositoryException(errorMsg, e);
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
                string errorMsg = $"Error updating tour log: {e.Message}";
                logger.Error(errorMsg);
                throw new TourLogRepositoryException(errorMsg, e);
            }
        }

        public async Task DeleteTourLogAsync(TourLog tourLog)
        {
            try
            {
                _dbContext.TourLogs.Remove(tourLog);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                string errorMsg = $"Error deleting tour log: {e.Message}";
                logger.Error(errorMsg);
                throw new TourLogRepositoryException(errorMsg, e);
            }
        }
    }
}
