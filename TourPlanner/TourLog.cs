using System;
using System.Collections.ObjectModel;

namespace TourPlanner
{
    public class TourLog
    {
        // Eigenschaften eines Tour-Logs
        public DateTime? Date { get; set; }
        public string? Comment { get; set; }
        public string? Difficulty { get; set; }
        public double TotalDistance { get; set; }
        public double TotalTime { get; set; }
        public string? Rating { get; set; }
        public string? Weather { get; set; }

        public ObservableCollection<TourLog> TourLogs { get; set; }

        // Konstruktor
        public TourLog()
        {
            // Initialisiere die Liste der Tour-Logs
            TourLogs = new ObservableCollection<TourLog>();

            // Füge einige Beispiel-Tour-Logs hinzu (kann optional sein)
            TourLogs.Add(new TourLog
            {
                Date = DateTime.Today,
                Comment = "First tour log",
                Difficulty = "Easy",
                TotalDistance = 10,
                TotalTime = 1,
                Rating = "3 Stars",
                Weather = "Sunny"
            });
            TourLogs.Add(new TourLog
            {
                Date = DateTime.Today.AddDays(-1),
                Comment = "Second tour log",
                Difficulty = "Moderate",
                TotalDistance = 15,
                TotalTime = 1.5,
                Rating = "4 Stars",
                Weather = "Cloudy"
            });
            TourLogs.Add(new TourLog
            {
                Date = DateTime.Today.AddDays(-2),
                Comment = "Third tour log",
                Difficulty = "Difficult",
                TotalDistance = 20,
                TotalTime = 2,
                Rating = "5 Stars",
                Weather = "Rainy"
            });
        }


    }
}