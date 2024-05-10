using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using TourPlanner.Views;
using TourPlannerBusinessLayer.Models;

namespace TourPlanner.ViewModels
{
    public class TourLogViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        // Commands
        public ICommand AddTourLogCommand { get; }
        public ICommand UpdateTourLogCommand { get; }
        public ICommand DeleteTourLogCommand { get; }

        // Eigenschaften
        private TourLog? selectedTourLog;
        public TourLog? SelectedTourLog
        {
            get { return selectedTourLog; }
            set
            {
                selectedTourLog = value;
                OnPropertyChanged(nameof(SelectedTourLog));
                LoadSelectedTourLogData();
            }
        }

        private Tour? selectedTour;
        public Tour? SelectedTour
        {
            get { return selectedTour; }
            set
            {
                selectedTour = value;
                OnPropertyChanged(nameof(SelectedTour));
            }
        }

        // Daten-Properties (Datum, Entfernung usw.)
        private DateTime? selectedDate;
        public DateTime? SelectedDate
        {
            get { return selectedDate; }
            set
            {
                selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        private double selectedTotalDistance;
        public double SelectedTotalDistance
        {
            get { return selectedTotalDistance; }
            set
            {
                selectedTotalDistance = value;
                OnPropertyChanged(nameof(SelectedTotalDistance));
            }
        }

        private double selectedTotalTime;
        public double SelectedTotalTime
        {
            get { return selectedTotalTime; }
            set
            {
                selectedTotalTime = value;
                OnPropertyChanged(nameof(SelectedTotalTime));
            }
        }

        private string? selectedComment;
        public string? SelectedComment
        {
            get { return selectedComment; }
            set
            {
                selectedComment = value;
                OnPropertyChanged(nameof(SelectedComment));
            }
        }

        private string? selectedDifficulty;
        public string? SelectedDifficulty
        {
            get { return selectedDifficulty; }
            set
            {
                selectedDifficulty = value;
                OnPropertyChanged(nameof(SelectedDifficulty));
            }
        }

        private string? selectedWeather;
        public string? SelectedWeather
        {
            get { return selectedWeather; }
            set
            {
                selectedWeather = value;
                OnPropertyChanged(nameof(SelectedWeather));
            }
        }

        public ObservableCollection<string> DifficultyOptions { get; } = new ObservableCollection<string>
        {
            "Easy",
            "Moderate",
            "Difficult",
            "Extreme"
        };

        public ObservableCollection<TourLog>? TourLogs { get; set; }

        private TourViewModel tourViewModel;

        public TourLogViewModel(TourViewModel tourViewModel)
        {
            this.tourViewModel = tourViewModel;
            AddTourLogCommand = new RelayCommand(AddNewTourLog);
            UpdateTourLogCommand = new RelayCommand(UpdateSelectedTourLog);
            DeleteTourLogCommand = new RelayCommand(DeleteSelectedTourLog);
            InitializeTourLogs();
        }

        // Initialisieren von Tour-Logs
        private void InitializeTourLogs()
        {
            TourLogs = new ObservableCollection<TourLog>(TourLog.CreateExampleTourLogs());
        }

        // Methoden zum Hinzufügen, Aktualisieren und Löschen
        public void AddNewTourLog(object? parameter)
        {
            var newTourLog = new TourLog
            {
                Date = SelectedDate,
                Comment = SelectedComment,
                Difficulty = SelectedDifficulty,
                TotalDistance = (int)SelectedTotalDistance,
                TotalTime = (int)SelectedTotalTime,
                Weather = SelectedWeather
            };

            if (newTourLog != null && SelectedTour != null)
            {
                SelectedTour.TourLogs.Add(newTourLog);
                OnPropertyChanged(nameof(SelectedTour.TourLogs));
            }
        }

        public void UpdateSelectedTourLog(object? parameter)
        {
            if (SelectedTourLog == null) return;

            SelectedTourLog.Date = SelectedDate;
            SelectedTourLog.Comment = SelectedComment;
            SelectedTourLog.Difficulty = SelectedDifficulty;
            SelectedTourLog.TotalDistance = (int)SelectedTotalDistance;
            SelectedTourLog.TotalTime = (int)SelectedTotalTime;
            SelectedTourLog.Weather = SelectedWeather;

            OnPropertyChanged(nameof(TourLogs));
        }

        public void DeleteSelectedTourLog(object? parameter)
        {
            if (SelectedTour == null || SelectedTourLog == null)
            {
                // Frühzeitig abbrechen, wenn keine ausgewählte Tour oder kein ausgewähltes TourLog vorhanden ist
                return;
            }

            // Suche das zu löschende TourLog in der Liste der TourLogs der ausgewählten Tour
            var tourLogToDelete = SelectedTour.TourLogs.FirstOrDefault(tl => tl.Id == SelectedTourLog.Id);

            if (tourLogToDelete != null)
            {
                // Wenn das TourLog gefunden wird, entferne es aus der Liste
                SelectedTour.TourLogs.Remove(tourLogToDelete);
                OnPropertyChanged(nameof(SelectedTour.TourLogs));
            }

            // Nach dem Löschen das ausgewählte TourLog zurücksetzen
            SelectedTourLog = null;
            OnPropertyChanged(nameof(SelectedTourLog));
        }




        // Daten laden (zum Anzeigen/Aktualisieren)
        private void LoadSelectedTourLogData()
        {
            SelectedDate = SelectedTourLog?.Date;
            SelectedComment = SelectedTourLog?.Comment;
            SelectedTotalDistance = SelectedTourLog?.TotalDistance ?? 0;
            SelectedTotalTime = SelectedTourLog?.TotalTime ?? 0;
            SelectedDifficulty = SelectedTourLog?.Difficulty;
            SelectedWeather = SelectedTourLog?.Weather;
        }

        public void OpenTourLogWindow(object parameter)
        {
            // Logik zum Hinzufügen einer Tour
            AddTourLogWindow addLogTourWindow = new AddTourLogWindow(this);
            addLogTourWindow.ShowDialog();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
