using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TourPlannerBusinessLayer.Exceptions;

namespace TourPlannerBusinessLayer.Service
{
    public class GeocodeService
    {
        private readonly HttpClient _httpClient;

        public GeocodeService()
        {
           
        }

        public GeocodeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public virtual async Task<(double, double)> GetCoordinatesAsync(string address, string apiKey)
        {
            if (address == null)
            {
                throw new GeocodeServiceException("Address cannot be null");
            }

            string url = $"https://api.openrouteservice.org/geocode/search?api_key={apiKey}&text={Uri.EscapeDataString(address)}";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(result);
                var coordinates = json["features"][0]["geometry"]["coordinates"];
                double longitude = coordinates[0].Value<double>();
                double latitude = coordinates[1].Value<double>();
                return (longitude, latitude);
            }
            catch (Exception ex)
            {
                throw new GeocodeServiceException("Error fetching geocode data", ex);
            }
        }
    }
}
