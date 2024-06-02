using Microsoft.Extensions.DependencyInjection;
using Models;
using TourPlannerBusinessLayer.Exceptions;
using TourPlannerDAL;

namespace TourPlannerBusinessLayer.Service
{
    public class TourLogService
    {
        private readonly IServiceProvider _serviceProvider;

        public TourLogService()
        {

        }
        public TourLogService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task AddTourLogAsync(TourLog tourLog)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var tourLogRepository = scope.ServiceProvider.GetRequiredService<TourLogRepository>();
                    await tourLogRepository.AddTourLogAsync(tourLog);
                }
            }
            catch (Exception ex)
            {
                throw new TourLogServiceException("Error adding tour log", ex);
            }
        }

        public async Task<List<TourLog>> GetTourLogsByTourIdAsync(int tourId)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var tourLogRepository = scope.ServiceProvider.GetRequiredService<TourLogRepository>();
                    return await tourLogRepository.GetTourLogsByTourIdAsync(tourId);
                }
            }
            catch (Exception ex)
            {
                throw new TourLogServiceException("Error retrieving tour logs by tour ID", ex);
            }
        }

        public async Task UpdateTourLogAsync(TourLog tourLogToUpdate)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var tourLogRepository = scope.ServiceProvider.GetRequiredService<TourLogRepository>();
                    await tourLogRepository.UpdateTourLogAsync(tourLogToUpdate);
                }
            }
            catch (Exception ex)
            {
                throw new TourLogServiceException("Error updating tour log", ex);
            }
        }

        public async Task DeleteTourLogAsync(TourLog tourLog)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var tourLogRepository = scope.ServiceProvider.GetRequiredService<TourLogRepository>();
                    await tourLogRepository.DeleteTourLogAsync(tourLog);
                }
            }
            catch (Exception ex)
            {
                throw new TourLogServiceException("Error deleting tour log", ex);
            }
        }
    }
}
