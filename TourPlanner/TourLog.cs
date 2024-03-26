using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TourPlanner
{
    public class TourLog : INotifyPropertyChanged
    {
        // Event für die Benachrichtigung über Änderungen
        public event PropertyChangedEventHandler? PropertyChanged;

        // Id-Eigenschaft hinzugefügt
        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        private DateTime? date;
        public DateTime? Date
        {
            get { return date; }
            set
            {
                if (date != value)
                {
                    date = value;
                    OnPropertyChanged(nameof(Date));
                }
            }
        }

        private string? comment;
        public string? Comment
        {
            get { return comment; }
            set
            {
                if (comment != value)
                {
                    comment = value;
                    OnPropertyChanged(nameof(Comment));
                }
            }
        }

        private string? difficulty;
        public string? Difficulty
        {
            get { return difficulty; }
            set
            {
                if (difficulty != value)
                {
                    difficulty = value;
                    OnPropertyChanged(nameof(Difficulty));
                }
            }
        }

        private double totalDistance;
        public double TotalDistance
        {
            get { return totalDistance; }
            set
            {
                if (totalDistance != value)
                {
                    totalDistance = value;
                    OnPropertyChanged(nameof(TotalDistance));
                }
            }
        }

        private double totalTime;
        public double TotalTime
        {
            get { return totalTime; }
            set
            {
                if (totalTime != value)
                {
                    totalTime = value;
                    OnPropertyChanged(nameof(TotalTime));
                }
            }
        }

        private string? rating;
        public string? Rating
        {
            get { return rating; }
            set
            {
                if (rating != value)
                {
                    rating = value;
                    OnPropertyChanged(nameof(Rating));
                }
            }
        }

        private string? weather;
        public string? Weather
        {
            get { return weather; }
            set
            {
                if (weather != value)
                {
                    weather = value;
                    OnPropertyChanged(nameof(Weather));
                }
            }
        }

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

        // Methode zum Auslösen des PropertyChanged-Events
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
