using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Models;
using TourPlannerBusinessLayer.Service;
using TourPlannerLogging;
using System.Diagnostics;

namespace TourPlannerBusinessLayer.Managers
{
    public class FileTransferManager
    {
        private readonly TourService _tourService;
        private readonly ILoggerWrapper logger = LoggerFactory.GetLogger();

        public FileTransferManager()
        {
        }
        public FileTransferManager(TourService tourService)
        {
            _tourService = tourService;
        }

        public async Task ExportToursAsync(string filePath)
        {
            try
            {
                List<Tour> tours = await _tourService.GetToursAsync();

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
                };

                string jsonString = JsonSerializer.Serialize(tours, options);
                await File.WriteAllTextAsync(filePath, jsonString);
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred while exporting tours: {ex.Message}");
                Debug.WriteLine($"An error occurred while exporting tours: {ex.Message}");
            }
        }

        public async Task ImportToursAsync(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string jsonString = await File.ReadAllTextAsync(filePath);

                    var options = new JsonSerializerOptions
                    {
                        ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
                    };

                    List<Tour> tours = JsonSerializer.Deserialize<List<Tour>>(jsonString, options);

                    foreach (var tour in tours)
                    {
                        // Setze die ID auf 0, damit die Datenbank eine neue ID generiert
                        tour.Id = 0;

                        foreach (var log in tour.TourLogs)
                        {
                            log.Id = 0;
                            log.TourId = 0; // Setze die TourId auf 0, damit die Datenbank eine neue ID generiert
                        }

                        await _tourService.AddTourAsync(tour);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred while importing tours: {ex.Message}");
                Debug.WriteLine($"An error occurred while importing tours: {ex.Message}");
            }
        }
    }
}
