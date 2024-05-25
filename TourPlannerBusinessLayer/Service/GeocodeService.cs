using Newtonsoft.Json.Linq;

namespace TourPlannerBusinessLayer.Service
{
    public class GeocodeService
    {
        private readonly HttpClient _httpClient;

        public GeocodeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(double, double)> GetCoordinatesAsync(string address){
            if (address != null){
                string apiKey = "5b3ce3597851110001cf6248a50f39187e9e47bd9876ff57c061a58f";
                string url = $"https://api.openrouteservice.org/geocode/search?api_key={apiKey}&text={Uri.EscapeDataString(address)}";

                try{
                    HttpResponseMessage response = await _httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string result = await response.Content.ReadAsStringAsync();
                    var json = JObject.Parse(result);
                    var coordinates = json["features"][0]["geometry"]["coordinates"];
                    double longitude = coordinates[0].Value<double>();
                    double latitude = coordinates[1].Value<double>();
                    return (longitude, latitude);
                }
                catch (Exception ex){
                    Console.WriteLine($"Error fetching geocode data: {ex.Message}");
                    return (0, 0);
                }
            }
            else{
                return (0, 0);
            }
        }
    }
}