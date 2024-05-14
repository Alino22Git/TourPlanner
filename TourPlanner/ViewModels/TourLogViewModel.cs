using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TourPlanner.Views;
using TourPlannerBusinessLayer.Models;

namespace TourPlanner.ViewModels
{
    public class TourLogViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand SaveTourLogCommand { get; }
        public ICommand DeleteTourLogCommand { get; }

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
                UpdateTourLogs();
            }
        }

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

        private ObservableCollection<TourLog>? tourLogs;
        public ObservableCollection<TourLog>? TourLogs
        {
            get { return tourLogs; }
            set
            {
                tourLogs = value;
                OnPropertyChanged(nameof(TourLogs));
            }
        }

        public ObservableCollection<string> DifficultyOptions { get; } = new ObservableCollection<string>
        {
            "Easy",
            "Moderate",
            "Difficult",
            "Extreme"
        };

        private TourViewModel tourViewModel;

        public TourLogViewModel(TourViewModel tourViewModel)
        {
            this.tourViewModel = tourViewModel;
            SaveTourLogCommand = new RelayCommand(SaveTourLog);
            DeleteTourLogCommand = new RelayCommand(DeleteSelectedTourLog);
            InitializeTourLogs();
        }

        private void InitializeTourLogs()
        {
            TourLogs = new ObservableCollection<TourLog>(TourLog.CreateExampleTourLogs());
        }

        private void UpdateTourLogs()
        {
            if (SelectedTour != null)
            {
                TourLogs = new ObservableCollection<TourLog>(SelectedTour.TourLogs);
            }
            else
            {
                TourLogs?.Clear();
            }
        }

        private void SaveTourLog(object? parameter)
        {
            if (SelectedTour == null)
            {
                return;
            }

            if (SelectedTourLog == null)
            {
                // Add new TourLog
                var newTourLog = new TourLog
                {
                    Date = SelectedDate,
                    Comment = SelectedComment,
                    Difficulty = SelectedDifficulty,
                    TotalDistance = (int)SelectedTotalDistance,
                    TotalTime = (int)SelectedTotalTime,
                    Weather = SelectedWeather
                };

                SelectedTour.TourLogs.Add(newTourLog);
            }
            else
            {
                // Update existing TourLog
                var tourLogToUpdate = SelectedTour.TourLogs.FirstOrDefault(tl => tl.Id == SelectedTourLog.Id);

                if (tourLogToUpdate != null)
                {
                    tourLogToUpdate.Date = SelectedDate;
                    tourLogToUpdate.Comment = SelectedComment;
                    tourLogToUpdate.Difficulty = SelectedDifficulty;
                    tourLogToUpdate.TotalDistance = (int)SelectedTotalDistance;
                    tourLogToUpdate.TotalTime = (int)SelectedTotalTime;
                    tourLogToUpdate.Weather = SelectedWeather;
                }
            }

            OnPropertyChanged(nameof(TourLogs));

            // Close the window after saving
            if (parameter is Window window)
            {
                window.DialogResult = true;
                window.Close();
            }
        }

        public void DeleteSelectedTourLog(object? parameter)
        {
            if (SelectedTour == null || SelectedTourLog == null)
            {
                return;
            }

            var tourLogToDelete = SelectedTour.TourLogs.FirstOrDefault(tl => tl.Id == SelectedTourLog.Id);

            if (tourLogToDelete != null)
            {
                SelectedTour.TourLogs.Remove(tourLogToDelete);
                OnPropertyChanged(nameof(TourLogs));
            }

            SelectedTourLog = null;
            OnPropertyChanged(nameof(SelectedTourLog));
        }

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
            if (SelectedTourLog == null)
            {
                // Create a new TourLog
                SelectedTourLog = new TourLog();
                LoadSelectedTourLogData();
            }

            var addTourLogWindow = new AddTourLogWindow(this);
            addTourLogWindow.ShowDialog();

            // Reset the SelectedTourLog after closing the window
            SelectedTourLog = null;
            LoadSelectedTourLogData();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
