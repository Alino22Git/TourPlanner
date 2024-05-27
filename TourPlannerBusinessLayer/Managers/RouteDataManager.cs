using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using TourPlannerBusinessLayer.Service;

namespace TourPlannerBusinessLayer.Managers
{
    public class RouteDataManager
    {
        private readonly GeocodeService _geocodeService;
        private readonly DirectionService _directionService;
        private readonly string _apiKey;

        public RouteDataManager(GeocodeService geocodeService, DirectionService directionService, IConfiguration configuration)
        {
            _geocodeService = geocodeService;
            _directionService = directionService;
            _apiKey = configuration["ApiKeys:OpenRouteService"];
        }

        public async Task<(double startLongitude, double startLatitude, double endLongitude, double endLatitude, string directions)> GetTourDataAsync(string from, string to)
        {
            var (startLongitude, startLatitude) = await _geocodeService.GetCoordinatesAsync(from, _apiKey);
            var (endLongitude, endLatitude) = await _geocodeService.GetCoordinatesAsync(to, _apiKey);
            string directions = await _directionService.GetDirectionsAsync(startLongitude, startLatitude, endLongitude, endLatitude, _apiKey);
            return (startLongitude, startLatitude, endLongitude, endLatitude, directions);
        }

        public async Task SaveDirectionsToFileAsync(string directions, string filePath)
        {
            string jsContent = $"const directions = {directions};";
            await File.WriteAllTextAsync(filePath, jsContent);
        }

        public string GetProjectResourcePath(string relativePath)
        {
            string projectDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            string resourceDir = Path.Combine(projectDir, "Resources");
            Directory.CreateDirectory(resourceDir);
            return Path.Combine(resourceDir, relativePath);
        }

        public async Task<(string Distance, string Duration)> GetDistanceAndDurationAsync(string from, string to, string transportType)
        {
            var (startLongitude, startLatitude) = await _geocodeService.GetCoordinatesAsync(from, _apiKey);
            var (endLongitude, endLatitude) = await _geocodeService.GetCoordinatesAsync(to, _apiKey);
            var (distance, duration) = await _directionService.GetRouteDistanceAndDurationAsync(startLongitude, startLatitude, endLongitude, endLatitude, transportType, _apiKey);
            return (distance, duration);
        }
    }
}
