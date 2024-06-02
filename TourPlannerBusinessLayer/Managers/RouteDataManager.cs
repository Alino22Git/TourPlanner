using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using TourPlannerBusinessLayer.Service;
using TourPlannerLogging;
namespace TourPlannerBusinessLayer.Managers
{
    public class RouteDataManager
    {
        private readonly GeocodeService _geocodeService;
        private readonly DirectionService _directionService;
        private readonly string _apiKey;
        private static readonly ILoggerWrapper logger = LoggerFactory.GetLogger();

        public RouteDataManager()
        {

        }
        public RouteDataManager(GeocodeService geocodeService, DirectionService directionService, IConfiguration configuration)
        {
            _geocodeService = geocodeService;
            _directionService = directionService;
            _apiKey = configuration["ApiKeys:OpenRouteService"];
        }

        public virtual async Task<(double startLongitude, double startLatitude, double endLongitude, double endLatitude, string directions)> GetTourDataAsync(string from, string to, string transportType)
        {
            try {
                var (startLongitude, startLatitude) = await _geocodeService.GetCoordinatesAsync(from, _apiKey);
                var (endLongitude, endLatitude) = await _geocodeService.GetCoordinatesAsync(to, _apiKey);
                string directions = await _directionService.GetDirectionsAsync(startLongitude, startLatitude, endLongitude, endLatitude, _apiKey, transportType);
                return (startLongitude, startLatitude, endLongitude, endLatitude, directions);
            }
            catch (Exception ex)
            {
                logger.Error($"Error retireving tour Data {ex.Message}");
                return (0, 0, 0, 0, "");
            }
        }

        public async Task SaveDirectionsToFileAsync(string directions, string filePath)
        {
            try
            { 
                string jsContent = $"const directions = {directions};";
                await File.WriteAllTextAsync(filePath, jsContent);
                logger.Info($"Directions saved to file {filePath}");
            } catch (Exception ex)
            {
                logger.Error($"Error saving directions to file {ex.Message}");
            }
        }

        public string GetProjectResourcePath(string relativePath)
        {
            try
            {
                string projectDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
                string resourceDir = Path.Combine(projectDir, "Resources");
                Directory.CreateDirectory(resourceDir);
                return Path.Combine(resourceDir, relativePath);
            }
            catch (Exception ex)
            {
                logger.Error($"Error getting project resource path {ex.Message}");
                return "";
            }
        }

        public virtual async Task<(string Distance, string Duration)> GetDistanceAndDurationAsync(string from, string to, string transportType)
        {
            try
            {
                var (startLongitude, startLatitude) = await _geocodeService.GetCoordinatesAsync(from, _apiKey);
                var (endLongitude, endLatitude) = await _geocodeService.GetCoordinatesAsync(to, _apiKey);
                var (distance, duration) = await _directionService.GetRouteDistanceAndDurationAsync(startLongitude, startLatitude, endLongitude, endLatitude, transportType, _apiKey);
                return (distance, duration);
            }
            catch (Exception ex)
            {
                logger.Error($"Error getting distance and duration {ex.Message}");
                return ("", "");
            }
        }
    }
}
