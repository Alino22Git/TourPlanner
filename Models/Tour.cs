using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;

namespace Models
{
    public class Tour : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private ObservableCollection<TourLog> tourLogs;
        public ObservableCollection<TourLog> TourLogs
        {
            get => tourLogs;
            set
            {
                if (tourLogs != value)
                {
                    if (tourLogs != null)
                    {
                        tourLogs.CollectionChanged -= TourLogs_CollectionChanged;
                    }

                    tourLogs = value;
                    tourLogs.CollectionChanged += TourLogs_CollectionChanged;
                    OnPropertyChanged(nameof(TourLogs));
                    OnPropertyChanged(nameof(Popularity));
                    OnPropertyChanged(nameof(ChildFriendliness));
                }
            }
        }

        private void TourLogs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Popularity));
            OnPropertyChanged(nameof(ChildFriendliness));
        }

        public Tour()
        {
            TourLogs = new ObservableCollection<TourLog>();
            TourLogs.CollectionChanged += TourLogs_CollectionChanged;
        }

        public int Popularity
        {
            get => TourLogs.Count;
            set
            {
                OnPropertyChanged(nameof(Popularity));
            }
        }

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

        private string? name;
        public string? Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        private string? from;
        public string? From
        {
            get => from;
            set
            {
                if (from != value)
                {
                    from = value;
                    OnPropertyChanged(nameof(From));
                }
            }
        }

        private string? to;
        public string? To
        {
            get => to;
            set
            {
                if (to != value)
                {
                    to = value;
                    OnPropertyChanged(nameof(To));
                }
            }
        }

        private string? distance;
        public string? Distance
        {
            get => distance;
            set
            {
                if (distance != value)
                {
                    distance = value;
                    OnPropertyChanged(nameof(Distance));
                    OnPropertyChanged(nameof(ChildFriendliness));
                }
            }
        }

        private string? time;
        public string? Time
        {
            get => time;
            set
            {
                if (time != value)
                {
                    time = value;
                    OnPropertyChanged(nameof(Time));
                    OnPropertyChanged(nameof(ChildFriendliness));
                }
            }
        }

        private string? description;
        public string? Description
        {
            get => description;
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        private string transportType;
        public string TransportType
        {
            get => transportType;
            set
            {
                if (transportType != value)
                {
                    transportType = value;
                    OnPropertyChanged(nameof(TransportType));
                }
            }
        }

        public int ChildFriendliness
        {
            get
            {
                double totalDifficulty = 0;
                double maxDifficulty = 0;

                foreach (TourLog tourLog in TourLogs)
                {
                    switch (tourLog.Difficulty)
                    {
                        case "Easy":
                            totalDifficulty += 1;
                            maxDifficulty += 1;
                            break;
                        case "Moderate":
                            totalDifficulty += 2;
                            maxDifficulty += 2;
                            break;
                        case "Hard":
                            totalDifficulty += 3;
                            maxDifficulty += 3;
                            break;
                        case "Extreme":
                            totalDifficulty += 4;
                            maxDifficulty += 4;
                            break;
                    }
                }

                if (totalDifficulty == 0 || string.IsNullOrEmpty(Time) || string.IsNullOrEmpty(Distance))
                {
                    return 0;
                }

                string distanceString = Distance.Replace(" km", string.Empty).Trim();
                string timeString = Time.Replace(" h", string.Empty).Trim();

                CultureInfo germanCulture = new CultureInfo("de-DE");

                if (!double.TryParse(distanceString, NumberStyles.Float, germanCulture, out double distanceValue))
                {
                    Console.WriteLine($"Fehler beim Parsen der Distanz: {Distance}");
                    return 0;
                }

                if (!double.TryParse(timeString, NumberStyles.Float, germanCulture, out double timeValue))
                {
                    Console.WriteLine($"Fehler beim Parsen der Zeit: {Time}");
                    return 0;
                }

                // Calculate normalized difficulty
                double normalizedDifficulty = totalDifficulty / maxDifficulty;

                // Use logarithmic scale to normalize time and distance
                double normalizedTime = Math.Log(timeValue + 1);
                double normalizedDistance = Math.Log(distanceValue + 1);

                // Calculate friendliness
                double friendliness = (1 - normalizedDifficulty) * 0.5 + (1 / (1 + normalizedTime)) * 0.25 + (1 / (1 + normalizedDistance)) * 0.25;
                int result = (int)(friendliness * 100);

                // Check if result is in valid range
                result = result % 100;

                return result;
            }
            set
            {
                OnPropertyChanged(nameof(Popularity));
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
