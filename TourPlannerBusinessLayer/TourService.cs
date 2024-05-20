using System.Threading.Tasks;
using Models;
using TourPlannerDAL;

namespace TourPlannerBusinessLayer.Services
{
    public class TourService
    {
        private readonly TourRepository _tourRepository;

        public TourService(TourRepository tourRepository)
        {
            _tourRepository = tourRepository;
        }

        public async Task AddTourAsync(Tour tour)
        {
            await _tourRepository.AddTourAsync(tour);
        }

        public async Task<List<Tour>> GetToursAsync()
        {
            return await _tourRepository.GetToursAsync();
        }
        // Weitere Methoden für CRUD-Operationen können hier hinzugefügt werden
    }
}