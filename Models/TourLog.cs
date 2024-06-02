using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Models
{
    public class TourLog : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private int tourId;
        public int TourId
        {
            get { return tourId; }
            set
            {
                if (tourId != value)
                {
                    tourId = value;
                    OnPropertyChanged(nameof(TourId));
                }
            }
        }

        public Tour Tour { get; set; }

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
                    date = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : (DateTime?)null;
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

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
