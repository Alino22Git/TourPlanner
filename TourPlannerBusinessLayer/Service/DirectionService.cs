using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TourPlannerBusinessLayer.Service
{
    public class DirectionService
    {
        private readonly HttpClient _httpClient;

        public DirectionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetDirectionsAsync(double startLongitude, double startLatitude, double endLongitude, double endLatitude)
        {
            string apiKey = "5b3ce3597851110001cf62482aefcf75b95c4b94b6ec0bc33d9d3337";
            string url = $"https://api.openrouteservice.org/v2/directions/driving-car?api_key={apiKey}&start={startLongitude.ToString(System.Globalization.CultureInfo.InvariantCulture)},{startLatitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}&end={endLongitude.ToString(System.Globalization.CultureInfo.InvariantCulture)},{endLatitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}";

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
                Debug.WriteLine($"Error fetching directions data: {ex.Message}");
                return null;
            }
        }

        public async Task<string?> GetRouteDistanceAsync(double startLongitude, double startLatitude, double endLongitude, double endLatitude)
        {
            string apiKey = "5b3ce3597851110001cf62482aefcf75b95c4b94b6ec0bc33d9d3337";
            string url = $"https://api.openrouteservice.org/v2/directions/driving-car?api_key={apiKey}&start={startLongitude.ToString(System.Globalization.CultureInfo.InvariantCulture)},{startLatitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}&end={endLongitude.ToString(System.Globalization.CultureInfo.InvariantCulture)},{endLatitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}";

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
                            string distance = firstSegment.GetProperty("distance").GetDouble().ToString();
                            return distance;
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching directions data: {ex.Message}");
                return null;
            }
        }
    }
}
