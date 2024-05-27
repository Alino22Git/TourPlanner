using Microsoft.Extensions.Configuration;

namespace TourPlannerConfig
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfiguration _configuration;

        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionString()
        {
            return _configuration.GetConnectionString("DefaultConnection");
        }

        public T GetValue<T>(string key)
        {
            return _configuration.GetValue<T>(key);
        }
    }
}