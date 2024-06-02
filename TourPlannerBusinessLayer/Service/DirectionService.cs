using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TourPlannerLogging;
namespace TourPlannerBusinessLayer.Service
{
    public class DirectionService
    {
        private readonly HttpClient _httpClient;
        private static readonly ILoggerWrapper logger = LoggerFactory.GetLogger();
        public DirectionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetDirectionsAsync(double startLongitude, double startLatitude, double endLongitude, double endLatitude, string apiKey, string transportType)
        {

            string url;

            if (transportType != "Car" && transportType != "Bicycle" && transportType != "Hike")
            {
                return null;
            }
            else if (transportType == "Car")
            {
                url = $"https://api.openrouteservice.org/v2/directions/driving-car?api_key={apiKey}&start={startLongitude.ToString(System.Globalization.CultureInfo.InvariantCulture)},{startLatitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}&end={endLongitude.ToString(System.Globalization.CultureInfo.InvariantCulture)},{endLatitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}";
            }
            else if (transportType == "Hike")
            {
                url = $"https://api.openrouteservice.org/v2/directions/foot-hiking?api_key={apiKey}&start={startLongitude.ToString(System.Globalization.CultureInfo.InvariantCulture)},{startLatitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}&end={endLongitude.ToString(System.Globalization.CultureInfo.InvariantCulture)},{endLatitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}";

            }
            else if (transportType == "Bicycle")
            {
                url = $"https://api.openrouteservice.org/v2/directions/cycling-regular?api_key={apiKey}&start={startLongitude.ToString(System.Globalization.CultureInfo.InvariantCulture)},{startLatitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}&end={endLongitude.ToString(System.Globalization.CultureInfo.InvariantCulture)},{endLatitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}";

            }
            else
            {
                url = $"https://api.openrouteservice.org/v2/directions/driving-car?api_key={apiKey}&start={startLongitude.ToString(System.Globalization.CultureInfo.InvariantCulture)},{startLatitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}&end={endLongitude.ToString(System.Globalization.CultureInfo.InvariantCulture)},{endLatitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}";
            }

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Error fetching directions data: {response.StatusCode} - {response.ReasonPhrase} - {errorResponse}");
                    return null;
                }

                string result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (Exception ex)
            {
                logger.Error($"Error fetching directions data: {ex.Message}");
                Debug.WriteLine($"Error fetching directions data: {ex.Message}");
                return null;
            }
        }

        public async Task<(string? Distance, string? Duration)> GetRouteDistanceAndDurationAsync(double startLongitude, double startLatitude, double endLongitude, double endLatitude, string transportType, string apiKey)
        {
            string url;

            if (transportType != "Car" && transportType != "Bicycle" && transportType != "Hike")
            {
                return (null, null);
            }else if (transportType == "Car")
            {
                 url = $"https://api.openrouteservice.org/v2/directions/driving-car?api_key={apiKey}&start={startLongitude.ToString(System.Globalization.CultureInfo.InvariantCulture)},{startLatitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}&end={endLongitude.ToString(System.Globalization.CultureInfo.InvariantCulture)},{endLatitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}";
            }
            else if(transportType == "Hike")
            {
                 url = $"https://api.openrouteservice.org/v2/directions/foot-hiking?api_key={apiKey}&start={startLongitude.ToString(System.Globalization.CultureInfo.InvariantCulture)},{startLatitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}&end={endLongitude.ToString(System.Globalization.CultureInfo.InvariantCulture)},{endLatitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}";

            }else if (transportType == "Bicycle")
            {
                 url = $"https://api.openrouteservice.org/v2/directions/cycling-regular?api_key={apiKey}&start={startLongitude.ToString(System.Globalization.CultureInfo.InvariantCulture)},{startLatitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}&end={endLongitude.ToString(System.Globalization.CultureInfo.InvariantCulture)},{endLatitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}";

            }
            else
            {
                 url = $"https://api.openrouteservice.org/v2/directions/driving-car?api_key={apiKey}&start={startLongitude.ToString(System.Globalization.CultureInfo.InvariantCulture)},{startLatitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}&end={endLongitude.ToString(System.Globalization.CultureInfo.InvariantCulture)},{endLatitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}";
            }

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Error fetching directions data: {response.StatusCode} - {response.ReasonPhrase} - {errorResponse}");
                    return (null, null);
                }

                string result = await response.Content.ReadAsStringAsync();

                using (JsonDocument doc = JsonDocument.Parse(result))
                {
                    JsonElement root = doc.RootElement;
                    JsonElement features = root.GetProperty("features");
                    if (features.GetArrayLength() > 0)
                    {
                        JsonElement firstFeature = features[0];
                        JsonElement properties = firstFeature.GetProperty("properties");
                        JsonElement segments = properties.GetProperty("segments");
                        if (segments.GetArrayLength() > 0)
                        {
                            JsonElement firstSegment = segments[0];
                            double distance = firstSegment.GetProperty("distance").GetDouble();
                            double duration = firstSegment.GetProperty("duration").GetDouble();
                            double distanceInKm = distance / 1000;
                            double durationInHours = duration / 3600;
                            string distanceInKmString = distanceInKm.ToString("0.00") + " km";
                            string durationInHoursString = durationInHours.ToString("0.00") + " h";
                            return (distanceInKmString, durationInHoursString);
                        }
                    }
                }

                return (null, null);
            }
            catch (Exception ex)
            {
                logger.Error($"Error fetching directions data: {ex.Message}");
                Debug.WriteLine($"Error fetching directions data: {ex.Message}");
                return (null, null);
            }
        }

    }
}
