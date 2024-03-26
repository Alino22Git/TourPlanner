using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace TourPlanner
{
    public class TourViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private ObservableCollection<Tour>? tours;
        public ObservableCollection<Tour>? Tours
        {
            get { return tours; }
            set
            {
                if (tours != value)
                {
                    tours = value;
                    OnPropertyChanged(nameof(Tours));
                }
            }
        }

        private ObservableCollection<TourLog>? tourLogs;
        public ObservableCollection<TourLog>? TourLogs
        {
            get { return tourLogs; }
            set
            {
                if (tourLogs != value)
                {
                    tourLogs = value;
                    OnPropertyChanged(nameof(TourLogs));
                }
            }
        }

        public TourViewModel()
        {
            InitializeTours();
            InitializeTourLogs();
        }

        public void InitializeTours()
        {
            Tours = new ObservableCollection<Tour>()
            {
                new Tour { Id = 1, Name = "Tour 1", From = "Location 1", To = "Location 1", Distance = "10 km", Time = "2", Description = "Description 1" },
                new Tour { Id = 2, Name = "Tour 2", From = "Location 2", To = "Location 1", Distance = "15 km", Time = "12", Description = "Description 2" },
                new Tour { Id = 3, Name = "Tour 3", From = "Location 3", To = "Location 1", Distance = "20 km", Time = "4", Description = "Description 3" }
            };
        }

        private void InitializeTourLogs()
        {
            // Fügen Sie die Beispiel-Tour-Logs hinzu
            TourLogs = new ObservableCollection<TourLog>(TourLog.CreateExampleTourLogs());
        }

        public void AddTour(Tour newTour)
        {
            Debug.Assert(Tours != null, nameof(Tours) + " != null");
            Tours.Add(newTour);
            OnPropertyChanged(nameof(Tours)); // Aktualisieren der Ansicht
        }

        public void AddTourLog(TourLog newTourLog)
        {
            Debug.Assert(TourLogs != null, nameof(TourLogs) + " != null");
            TourLogs.Add(newTourLog);
        }

        public void UpdateTour(Tour? updatedTour)
        {
            Debug.Assert(Tours != null, nameof(Tours) + " != null");

            // Suchen Sie die zu aktualisierende Tour in der Liste
            Tour? existingTour = Tours.FirstOrDefault(tour => tour.Id == updatedTour?.Id);

            // Überprüfen, ob die Tour gefunden wurde
            if (existingTour != null)
            {
                // Aktualisieren Sie die Eigenschaften der vorhandenen Tour
                existingTour.Name = updatedTour?.Name;
                existingTour.From = updatedTour?.From;
                existingTour.To = updatedTour?.To;
                existingTour.Distance = updatedTour?.Distance;
                existingTour.Time = updatedTour?.Time;
                existingTour.Description = updatedTour?.Description;

                // Benachrichtigen Sie die UI über die Änderungen
                OnPropertyChanged(nameof(Tours));
            }
            else
            {
                // Wenn die Tour nicht gefunden wurde, geben Sie einen Fehler aus oder führen Sie eine andere geeignete Aktion aus
                Debug.WriteLine("Tour not found for update.");
            }
        }

        public void UpdateTourLog(TourLog selectedTourLog)
        {
            Debug.Assert(TourLogs != null, nameof(TourLogs) + " != null");

            // Suchen Sie das zu aktualisierende Tour-Log in der Liste
            TourLog? existingTourLog = TourLogs.FirstOrDefault(log => log.Id == selectedTourLog.Id);

            // Überprüfen, ob das Tour-Log gefunden wurde
            if (existingTourLog != null)
            {
                // Aktualisieren Sie die Eigenschaften des vorhandenen Tour-Logs
                // Hier implementieren Sie die Logik zum Aktualisieren des Tour-Logs basierend auf den übergebenen Daten

                // Benachrichtigen Sie die UI über die Änderungen
                OnPropertyChanged(nameof(TourLogs));
            }
            else
            {
                // Wenn das Tour-Log nicht gefunden wurde, geben Sie einen Fehler aus oder führen Sie eine andere geeignete Aktion aus
                Debug.WriteLine("Tour log not found for update.");
            }
        }


        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Tour? FindTourById(int id)
        {
            Debug.Assert(Tours != null, nameof(Tours) + " != null");
            return Tours.FirstOrDefault(tour => tour.Id == id);
        }
    }
}
