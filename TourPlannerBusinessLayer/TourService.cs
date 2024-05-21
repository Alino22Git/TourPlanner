using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Models;
using TourPlannerDAL;

namespace TourPlannerBusinessLayer.Services
{
    public class TourService
    {
        private readonly IServiceProvider _serviceProvider;

        public TourService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task AddTourAsync(Tour tour)
        {
            ValidateTour(tour);

            using (var scope = _serviceProvider.CreateScope())
            {
                var tourRepository = scope.ServiceProvider.GetRequiredService<TourRepository>();
                await tourRepository.AddTourAsync(tour);
            }
        }

        public async Task<List<Tour>> GetToursAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var tourRepository = scope.ServiceProvider.GetRequiredService<TourRepository>();
                return await tourRepository.GetToursAsync();
            }
        }

        public async Task AddTourLogAsync(TourLog tourLog)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var tourLogRepository = scope.ServiceProvider.GetRequiredService<TourLogRepository>();
                await tourLogRepository.AddTourLogAsync(tourLog);
            }
        }

        public async Task<List<TourLog>> GetTourLogsAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var tourLogRepository = scope.ServiceProvider.GetRequiredService<TourLogRepository>();
                return await tourLogRepository.GetTourLogsAsync();
            }
        }

        public async Task DeleteTourAsync(Tour tour)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var tourLogRepository = scope.ServiceProvider.GetRequiredService<TourLogRepository>();
                var tourRepository = scope.ServiceProvider.GetRequiredService<TourRepository>();

                // Löschen aller zugehörigen TourLogs
                //await tourLogRepository.DeleteTourLogsByTourIdAsync(tour.Id);

                // Löschen der Tour
                await tourRepository.DeleteTourAsync(tour);
            }
        }

        public async Task UpdateTourAsync(Tour tour)
        {
            ValidateTour(tour);

            using (var scope = _serviceProvider.CreateScope())
            {
                var tourRepository = scope.ServiceProvider.GetRequiredService<TourRepository>();
                await tourRepository.UpdateTourAsync(tour);
            }
        }

        private void ValidateTour(Tour tour)
        {
            if (string.IsNullOrEmpty(tour.Name))
                throw new ArgumentException("Tour Name cannot be empty");

            if (string.IsNullOrEmpty(tour.TransportType))
                throw new ArgumentException("Transport Type cannot be empty");
        }
    }
}
