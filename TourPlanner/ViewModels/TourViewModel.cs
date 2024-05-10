using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TourPlanner.Views;
using TourPlannerBusinessLayer.Models;

namespace TourPlanner.ViewModels
{
    public class TourViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        // Commands
        public ICommand SaveTourCommand { get; }
        public ICommand DeleteTourCommand { get; }

        // Eigenschaften für die Touren
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

        private Tour? selectedTour;
        public Tour? SelectedTour
        {
            get { return selectedTour; }
            set
            {
                if (selectedTour != value)
                {
                    selectedTour = value;
                    OnPropertyChanged(nameof(SelectedTour));
                }
            }
        }

        // Zum Bearbeiten einer bestehenden Tour
        private Tour? OriginalTour { get; set; }

        public TourViewModel()
        {
            InitializeTours();
            SaveTourCommand = new RelayCommand(SaveTour);
            DeleteTourCommand = new RelayCommand(DeleteSelectedTour);
        }

        private void InitializeTours()
        {
            // Beispiel-Touren erstellen
            Tours = new ObservableCollection<Tour>
            {
                new Tour { Id = 1, Name = "Tour 1", From = "Location 1", To = "Location 1", Distance = "10 km", Time = "2", Description = "Description 1", TourLogs = new ObservableCollection<TourLog>() },
                new Tour { Id = 2, Name = "Tour 2", From = "Location 2", To = "Location 1", Distance = "15 km", Time = "12", Description = "Description 2" },
                new Tour { Id = 3, Name = "Tour 3", From = "Location 3", To = "Location 1", Distance = "20 km", Time = "4", Description = "Description 3" }
            };

            // Beispiel-TourLogs hinzufügen, nur für die erste Tour
            var exampleTourLogs = TourLog.CreateExampleTourLogs();
            Tours[0].TourLogs = new ObservableCollection<TourLog>(exampleTourLogs);
        }

        // Startet die Bearbeitung der Tour und öffnet das AddTourWindow
        public void OpenTourWindow(Tour originalTour)
        {
            StartEditing(originalTour);
            var addTourWindow = new AddTourWindow(this, originalTour);
            addTourWindow.ShowDialog();
        }

        public void StartEditing(Tour originalTour)
        {
            if (originalTour == null)
            {

            }
            OriginalTour = originalTour;
            SelectedTour = new Tour
            {
                Id = originalTour.Id,
                Name = originalTour.Name,
                From = originalTour.From,
                To = originalTour.To,
                Distance = originalTour.Distance,
                Time = originalTour.Time,
                Description = originalTour.Description,
                TourLogs = new ObservableCollection<TourLog>(originalTour.TourLogs)
            };
            OnPropertyChanged(nameof(SelectedTour));
        }
        public void CreateNewTour(object? parameter)
        {
            OriginalTour = null;
            SelectedTour = new Tour();
            var addTourWindow = new AddTourWindow(this, SelectedTour);
            addTourWindow.ShowDialog();
        }
        private void SaveTour(object? parameter)
        {
            if (OriginalTour != null && SelectedTour != null)
            {
                OriginalTour.Name = SelectedTour.Name;
                OriginalTour.From = SelectedTour.From;
                OriginalTour.To = SelectedTour.To;
                OriginalTour.Distance = SelectedTour.Distance;
                OriginalTour.Time = SelectedTour.Time;
                OriginalTour.Description = SelectedTour.Description;

                if (!Tours.Contains(OriginalTour))
                {
                    Tours.Add(OriginalTour);
                }

                // Schließen des Fensters nach dem Speichern
                if (parameter is Window window)
                {
                    window.DialogResult = true;
                    window.Close();
                }
            }
        }

        private void DeleteSelectedTour(object? parameter)
        {
            if (SelectedTour != null && Tours != null)
            {
                // Finden der Tour, die gelöscht werden soll, anhand der Id
                var tourToDelete = Tours.FirstOrDefault(t => t.Id == SelectedTour.Id);

                // Nur löschen, wenn die Tour gefunden wird
                if (tourToDelete != null)
                {
                    Tours.Remove(tourToDelete);
                }

                // `SelectedTour` auf `null` setzen
                SelectedTour = null;

                // Aktualisieren Sie die UI-Komponenten
                OnPropertyChanged(nameof(Tours));
                OnPropertyChanged(nameof(SelectedTour));

                // Schließen Sie das Fenster, falls erforderlich
                if (parameter is Window window)
                {
                    window.DialogResult = true;
                    window.Close();
                }
            }
        }



        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Tour FindTourById(int tourId)
        {
            return Tours.FirstOrDefault(tour => tour.Id == tourId);
        }
    }
}
