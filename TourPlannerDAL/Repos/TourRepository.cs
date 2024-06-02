using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;
using TourPlannerLogging;
using TourPlannerDAL.Exceptions;

namespace TourPlannerDAL
{
    public class TourRepository
    {
        private readonly TourPlannerDbContext _dbContext;
        private static readonly ILoggerWrapper logger = LoggerFactory.GetLogger();

        public TourRepository(TourPlannerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddTourAsync(Tour tour)
        {
            try
            {
                _dbContext.Tours.Add(tour);
                await _dbContext.SaveChangesAsync();
                await _dbContext.Entry(tour).GetDatabaseValuesAsync();
            }
            catch (Exception e)
            {
                string errorMsg = $"Error adding tour: {e.Message}";
                logger.Error(errorMsg);
                throw new TourRepositoryException(errorMsg, e);
            }
        }

        public async Task UpdateTourAsync(Tour tour)
        {
            try
            {
                _dbContext.Tours.Update(tour);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                string errorMsg = $"Error updating tour: {e.Message}";
                logger.Error(errorMsg);
                throw new TourRepositoryException(errorMsg, e);
            }
        }

        public async Task DeleteTourAsync(Tour tour)
        {
            try
            {
                var tourLogs = await _dbContext.TourLogs.Where(tl => tl.TourId == tour.Id).ToListAsync();
                _dbContext.TourLogs.RemoveRange(tourLogs);
                _dbContext.Tours.Remove(tour);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                string errorMsg = $"Error deleting tour: {e.Message}";
                logger.Error(errorMsg);
                throw new TourRepositoryException(errorMsg, e);
            }
        }

        public async Task<List<Tour>> GetToursAsync()
        {
            try
            {
                return await _dbContext.Tours.Include(t => t.TourLogs).ToListAsync();
            }
            catch (Exception e)
            {
                string errorMsg = $"Error retrieving tours: {e.Message}";
                logger.Error(errorMsg);
                throw new TourRepositoryException(errorMsg, e);
            }
        }
    }
}
