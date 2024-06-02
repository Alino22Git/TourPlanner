using Microsoft.Extensions.DependencyInjection;
using Models;
using TourPlannerBusinessLayer.Exceptions;
using TourPlannerDAL;

namespace TourPlannerBusinessLayer.Service
{
    public class TourService
    {
        private readonly IServiceProvider _serviceProvider;

        public TourService()
        {
        }

        public TourService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public virtual async Task AddTourAsync(Tour tour)
        {
            try
            {
                ValidateTour(tour);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var tourRepository = scope.ServiceProvider.GetRequiredService<TourRepository>();
                    await tourRepository.AddTourAsync(tour);
                }
            }
            catch (Exception ex)
            {
                throw new TourServiceException("Error adding tour", ex);
            }
        }

        public virtual async Task<List<Tour>> GetToursAsync()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var tourRepository = scope.ServiceProvider.GetRequiredService<TourRepository>();
                    return await tourRepository.GetToursAsync();
                }
            }
            catch (Exception ex)
            {
                throw new TourServiceException("Error retrieving tours", ex);
            }
        }

        public virtual async Task AddTourLogAsync(TourLog tourLog)
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
                throw new TourServiceException("Error adding tour log", ex);
            }
        }

        public virtual async Task<List<TourLog>> GetTourLogsAsync()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var tourLogRepository = scope.ServiceProvider.GetRequiredService<TourLogRepository>();
                    return await tourLogRepository.GetTourLogsAsync();
                }
            }
            catch (Exception ex)
            {
                throw new TourServiceException("Error retrieving tour logs", ex);
            }
        }

        public virtual async Task DeleteTourAsync(Tour tour)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var tourRepository = scope.ServiceProvider.GetRequiredService<TourRepository>();
                    await tourRepository.DeleteTourAsync(tour);
                }
            }
            catch (Exception ex)
            {
                throw new TourServiceException("Error deleting tour", ex);
            }
        }

        public virtual async Task UpdateTourAsync(Tour tour)
        {
            try
            {
                ValidateTour(tour);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var tourRepository = scope.ServiceProvider.GetRequiredService<TourRepository>();
                    await tourRepository.UpdateTourAsync(tour);
                }
            }
            catch (Exception ex)
            {
                throw new TourServiceException("Error updating tour", ex);
            }
        }

        private void ValidateTour(Tour tour)
        {
            if (string.IsNullOrEmpty(tour.Name))
                throw new ValidationException("Tour Name cannot be empty");

            if (string.IsNullOrEmpty(tour.TransportType))
                throw new ValidationException("Transport Type cannot be empty");
        }
    }
}
