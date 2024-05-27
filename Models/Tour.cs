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
                // Der Setter bleibt leer, weil Popularity basierend auf TourLogs berechnet wird
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
                foreach (TourLog tourLog in TourLogs)
                {
                    switch (tourLog.Difficulty)
                    {
                        case "Easy":
                            totalDifficulty += 1;
                            break;
                        case "Moderate":
                            totalDifficulty += 2;
                            break;
                        case "Hard":
                            totalDifficulty += 3;
                            break;
                        case "Extreme":
                            totalDifficulty += 4;
                            break;
                    }
                }

                if (totalDifficulty == 0 || string.IsNullOrEmpty(Time) || string.IsNullOrEmpty(Distance))
                {
                    return 0;
                }

                // Entfernen Sie die Einheit aus der Distanz- und Zeit-Strings
                string distanceString = Distance.Replace(" km", string.Empty).Trim();
                string timeString = Time.Replace(" h", string.Empty).Trim();

                // Versuchen Sie, die bereinigten Strings mit der deutschen Kultur zu parsen
                CultureInfo germanCulture = new CultureInfo("de-DE");

                if (!double.TryParse(distanceString, NumberStyles.Float, germanCulture, out double distanceValue))
                {
                    // Wenn das Parsen fehlschlägt, geben Sie eine Fehlermeldung aus
                    Console.WriteLine($"Fehler beim Parsen der Distanz: {Distance}");
                    return 0;
                }

                if (!double.TryParse(timeString, NumberStyles.Float, germanCulture, out double timeValue))
                {
                    // Wenn das Parsen fehlschlägt, geben Sie eine Fehlermeldung aus
                    Console.WriteLine($"Fehler beim Parsen der Zeit: {Time}");
                    return 0;
                }

                int result = (int)(100000000 / (totalDifficulty * timeValue * distanceValue));
                // Berechnung der ChildFriendliness
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
