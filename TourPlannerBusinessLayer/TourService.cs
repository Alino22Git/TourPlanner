using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
            using (var scope = _serviceProvider.CreateScope())
            {
                var tourRepository = scope.ServiceProvider.GetRequiredService<TourRepository>();
                await tourRepository.AddTourAsync(tour);

                // Hier sicherstellen, dass die Tour-ID aktualisiert wird
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
                var tourRepository = scope.ServiceProvider.GetRequiredService<TourRepository>();
                await tourRepository.DeleteTourAsync(tour);
            }
        }

        public async Task UpdateTourAsync(Tour tour)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var tourRepository = scope.ServiceProvider.GetRequiredService<TourRepository>();
                await tourRepository.UpdateTourAsync(tour);
            }
        }
    }
}