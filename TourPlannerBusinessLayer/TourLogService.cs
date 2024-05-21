using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Models;
using TourPlannerDAL;

namespace TourPlannerBusinessLayer.Services
{
    public class TourLogService
    {
        private readonly IServiceProvider _serviceProvider;

        public TourLogService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task AddTourLogAsync(TourLog tourLog)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var tourLogRepository = scope.ServiceProvider.GetRequiredService<TourLogRepository>();
                await tourLogRepository.AddTourLogAsync(tourLog);
            }
        }

        public async Task<List<TourLog>> GetTourLogsByTourIdAsync(int tourId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var tourLogRepository = scope.ServiceProvider.GetRequiredService<TourLogRepository>();
                return await tourLogRepository.GetTourLogsByTourIdAsync(tourId);
            }
        }

        public async Task UpdateTourLogAsync(TourLog tourLogToUpdate)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var tourLogRepository = scope.ServiceProvider.GetRequiredService<TourLogRepository>();
                await tourLogRepository.UpdateTourLogAsync(tourLogToUpdate);
            }
        }

        public async Task DeleteTourLogAsync(TourLog tourLog)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var tourLogRepository = scope.ServiceProvider.GetRequiredService<TourLogRepository>();
                await tourLogRepository.DeleteTourLogAsync(tourLog);
            }
        }
    }
}