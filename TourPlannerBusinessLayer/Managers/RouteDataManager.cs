using TourPlannerBusinessLayer.Service;

namespace TourPlannerBusinessLayer.Managers
{
    public class RouteDataManager
    {
        private readonly GeocodeService _geocodeService;
        private readonly DirectionService _directionService;

        public RouteDataManager(GeocodeService geocodeService, DirectionService directionService){
            _geocodeService = geocodeService;
            _directionService = directionService;
        }

        public async Task<(double startLongitude, double startLatitude, double endLongitude, double endLatitude, string directions)> GetTourDataAsync(string from, string to){
            var (startLongitude, startLatitude) = await _geocodeService.GetCoordinatesAsync(from);
            var (endLongitude, endLatitude) = await _geocodeService.GetCoordinatesAsync(to);
            string directions = await _directionService.GetDirectionsAsync(startLongitude, startLatitude, endLongitude, endLatitude);
            return (startLongitude, startLatitude, endLongitude, endLatitude, directions);
        }

        public async Task SaveDirectionsToFileAsync(string directions, string filePath){
            string jsContent = $"const directions = {directions};";
            await File.WriteAllTextAsync(filePath, jsContent);
        }

        public string GetProjectResourcePath(string relativePath){
            string projectDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            string resourceDir = Path.Combine(projectDir, "Resources");
            Directory.CreateDirectory(resourceDir); 
            return Path.Combine(resourceDir, relativePath);
        }

        public async Task<string> GetDistanceAsync(string from, string to)
        {
            var (startLongitude, startLatitude) = await _geocodeService.GetCoordinatesAsync(from);
            var (endLongitude, endLatitude) = await _geocodeService.GetCoordinatesAsync(to);
            string distance = await _directionService.GetRouteDistanceAsync(startLongitude, startLatitude, endLongitude, endLatitude);
            return distance;
        }
    }
}