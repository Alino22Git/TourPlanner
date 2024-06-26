﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Models;
using TourPlanner.ViewModels;
using TourPlanner.Views;
using TourPlannerBusinessLayer.Service;
using TourPlannerBusinessLayer.Exceptions;
using TourPlannerLogging;
using TourPlanner.Exceptions;

namespace TourPlanner.Viewmodels
{
    public class TourLogViewModel : INotifyPropertyChanged
    {
        private readonly TourLogService _tourLogService;
        private readonly TourService _tourService;
        private static readonly ILoggerWrapper _logger = LoggerFactory.GetLogger();

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

        private ObservableCollection<WeatherOption> weatherOptions;
        public ObservableCollection<WeatherOption> WeatherOptions
        {
            get { return weatherOptions; }
            set
            {
                weatherOptions = value;
                OnPropertyChanged(nameof(WeatherOptions));
            }
        }

        private string selectedRating;
        public string SelectedRating
        {
            get { return selectedRating; }
            set
            {
                selectedRating = value;
                OnPropertyChanged(nameof(SelectedRating));
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

        public ObservableCollection<string> DifficultyOptions { get; } = new ObservableCollection<string>{
            "Easy",
            "Moderate",
            "Difficult",
            "Extreme"
        };

        public ObservableCollection<String> RatingOptions { get; } = new ObservableCollection<string>{
            "1",
            "2",
            "3",
            "4",
            "5"
        };

        private TourViewModel tourViewModel;


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public TourLogViewModel(TourViewModel tourViewModel, TourLogService tourLogService, TourService tourService)
        {
            this.tourViewModel = tourViewModel;
            this._tourLogService = tourLogService;
            this._tourService = tourService;
            SaveTourLogCommand = new RelayCommand(async (parameter) => await SaveTourLog(parameter));
            DeleteTourLogCommand = new RelayCommand(async (parameter) => await DeleteSelectedTourLog(parameter));
            InitializeWeatherOptions();
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void InitializeWeatherOptions()
        {
            WeatherOptions = new ObservableCollection<WeatherOption>
            {
                new WeatherOption { Name = "Sunny" },
                new WeatherOption { Name = "Rainy" },
                new WeatherOption { Name = "Cloudy" },
                new WeatherOption { Name = "Snowy" },
                new WeatherOption { Name = "Stormy" }
            };
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private async void UpdateTourLogs()
        {
            if (SelectedTour != null)
            {
                try
                {
                    var logs = await _tourLogService.GetTourLogsByTourIdAsync(SelectedTour.Id);
                    TourLogs = new ObservableCollection<TourLog>(logs);
                }
                catch (Exception ex)
                {
                    _logger.Error($"Error updating tour logs: {ex.Message}");
                    MessageBox.Show($"Error updating tour logs: {ex.Message}");
                    throw new UpdateException("Error updating tour logs", ex);
                }
            }
            else
            {
                TourLogs?.Clear();
            }
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private async Task SaveTourLog(object? parameter)
        {
            if (SelectedTour == null)
            {
                return;
            }

            var selectedWeatherOptions = WeatherOptions.Where(w => w.IsChecked).Select(w => w.Name).ToList();
            var selectedWeather = string.Join(", ", selectedWeatherOptions);

            try
            {
                if (SelectedTourLog == null)
                {
                    var newTourLog = new TourLog
                    {
                        Date = SelectedDate.HasValue ? DateTime.SpecifyKind(SelectedDate.Value, DateTimeKind.Utc) : (DateTime?)null,
                        Comment = SelectedComment,
                        Difficulty = SelectedDifficulty,
                        TotalDistance = SelectedTotalDistance,
                        TotalTime = SelectedTotalTime,
                        Weather = selectedWeather,
                        Rating = SelectedRating,
                        TourId = SelectedTour.Id
                    };

                    await _tourLogService.AddTourLogAsync(newTourLog);
                    SelectedTour.TourLogs.Add(newTourLog);
                    await _tourService.UpdateTourAsync(SelectedTour);
                }
                else
                {
                    var tourLogToUpdate = SelectedTour.TourLogs.FirstOrDefault(tl => tl.Id == SelectedTourLog.Id);

                    if (tourLogToUpdate != null)
                    {
                        tourLogToUpdate.Date = SelectedDate.HasValue ? DateTime.SpecifyKind(SelectedDate.Value, DateTimeKind.Utc) : (DateTime?)null;
                        tourLogToUpdate.Comment = SelectedComment;
                        tourLogToUpdate.Difficulty = SelectedDifficulty;
                        tourLogToUpdate.TotalDistance = SelectedTotalDistance;
                        tourLogToUpdate.TotalTime = SelectedTotalTime;
                        tourLogToUpdate.Weather = selectedWeather;
                        tourLogToUpdate.Rating = SelectedRating;
                        await _tourLogService.UpdateTourLogAsync(tourLogToUpdate);
                        await _tourService.UpdateTourAsync(SelectedTour);
                    }
                }

                if (parameter is Window window)
                {
                    window.DialogResult = true;
                    window.Close();
                }
                UpdateTourLogs();
            }
            catch (Exception ex)
            {
                _logger.Error($"Error saving tour log: {ex.Message}");
                MessageBox.Show($"Error saving tour log: {ex.Message}");
                throw new SaveException("Error saving tour log", ex);
            }
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public async Task DeleteSelectedTourLog(object? parameter)
        {
            if (SelectedTour == null || SelectedTourLog == null)
            {
                return;
            }

            try
            {
                var tourLogToDelete = SelectedTour.TourLogs.FirstOrDefault(tl => tl.Id == SelectedTourLog.Id);

                if (tourLogToDelete != null)
                {
                    SelectedTour.TourLogs.Remove(tourLogToDelete);
                    await _tourLogService.DeleteTourLogAsync(tourLogToDelete);
                    OnPropertyChanged(nameof(TourLogs));
                    await _tourService.UpdateTourAsync(SelectedTour);
                }
                SelectedTourLog = null;
                UpdateTourLogs();
                OnPropertyChanged(nameof(SelectedTourLog));
                if (parameter is Window window)
                {
                    window.DialogResult = true;
                    window.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error deleting tour log: {ex.Message}");
                MessageBox.Show($"Error deleting tour log: {ex.Message}");
                throw new DeleteException("Error deleting tour log", ex);
            }
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void LoadSelectedTourLogData()
        {
            try
            {
                SelectedDate = SelectedTourLog?.Date;
                SelectedComment = SelectedTourLog?.Comment;
                SelectedTotalDistance = SelectedTourLog?.TotalDistance ?? 0;
                SelectedTotalTime = SelectedTourLog?.TotalTime ?? 0;
                SelectedDifficulty = SelectedTourLog?.Difficulty;

                foreach (var option in WeatherOptions)
                {
                    option.IsChecked = SelectedTourLog?.Weather?.Split(',').Contains(option.Name.Trim()) ?? false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error loading selected tour log data: {ex.Message}");
                MessageBox.Show($"Error loading selected tour log data: {ex.Message}");
                throw new LoadException("Error loading selected tour log data", ex);
            }
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
