using System.Collections.Generic;
using System;

namespace TourPlanner
{
    public class TourLog
    {
        // Eigenschaften eines Tour-Logs
        public int Id { get; set; } // Id-Eigenschaft hinzugefügt
        public DateTime? Date { get; set; }
        public string? Comment { get; set; }
        public string? Difficulty { get; set; }
        public double TotalDistance { get; set; }
        public double TotalTime { get; set; }
        public string? Rating { get; set; }
        public string? Weather { get; set; }

        // Eine Liste von ausgewählten Touren für das Tour-Log
        public List<Tour> SelectedTours { get; set; }

        // Konstruktor
        public TourLog()
        {
            // Initialisiere die Liste der ausgewählten Touren
            SelectedTours = new List<Tour>();
        }

        // Methode zum Erstellen von Beispiel-Tour-Logs
        public static List<TourLog> CreateExampleTourLogs()
        {
            var exampleLogs = new List<TourLog>();

            exampleLogs.Add(new TourLog
            {
                Id = 1,
                Date = DateTime.Today,
                Comment = "First tour log",
                Difficulty = "Easy",
                TotalDistance = 10,
                TotalTime = 1,
                Rating = "3 Stars",
                Weather = "Sunny"
            });
            exampleLogs.Add(new TourLog
            {
                Id = 2,
                Date = DateTime.Today.AddDays(-1),
                Comment = "Second tour log",
                Difficulty = "Moderate",
                TotalDistance = 15,
                TotalTime = 1.5,
                Rating = "4 Stars",
                Weather = "Cloudy"
            });
            exampleLogs.Add(new TourLog
            {
                Id = 3,
                Date = DateTime.Today.AddDays(-2),
                Comment = "Third tour log",
                Difficulty = "Difficult",
                TotalDistance = 20,
                TotalTime = 2,
                Rating = "5 Stars",
                Weather = "Rainy"
            });

            return exampleLogs;
        }
    }
}