using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Models;
using TourPlanner.Views;
using TourPlannerBusinessLayer.Managers;
using TourPlannerBusinessLayer.Service;

namespace TourPlanner.ViewModels
{
    public class TourViewModel : INotifyPropertyChanged
    {
        private readonly TourService _tourService;
        private readonly RouteDataManager _routeDataManager;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand SaveTourCommand { get; }
        public ICommand DeleteTourCommand { get; }

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

        public ObservableCollection<string> TransportTypeOptions { get; } = new ObservableCollection<string>{
            "Hike",
            "Bicycle",
            "Car"
        };

        private Tour? OriginalTour { get; set; }

        public TourViewModel(TourService tourService, RouteDataManager routeDataManager)
        {
            _tourService = tourService ?? throw new ArgumentNullException(nameof(tourService));
            _routeDataManager = routeDataManager ?? throw new ArgumentNullException(nameof(routeDataManager));
            LoadTours();
            SaveTourCommand = new RelayCommand(async (parameter) => await SaveTour(parameter));
            DeleteTourCommand = new RelayCommand(async (parameter) => await DeleteSelectedTour(parameter));
        }

        public async void LoadTours()
        {
            var toursFromDb = await _tourService.GetToursAsync();
            Tours = new ObservableCollection<Tour>(toursFromDb);
        }

        public async void StartEditing(Tour originalTour)
        {
            if (originalTour == null) return;

            OriginalTour = originalTour;
            var distance = string.Empty;
            var time = string.Empty;

            if (!string.IsNullOrEmpty(originalTour.From) && !string.IsNullOrEmpty(originalTour.To))
            {
                try
                {
                    (distance, time) = await _routeDataManager.GetDistanceAndDurationAsync(originalTour.From, originalTour.To, originalTour.TransportType);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error getting distance and time for tour {originalTour.Name}: {ex.Message}");
                    distance = null; 
                    time = null;
                }
            }

            SelectedTour = new Tour
            {
                Id = originalTour.Id,
                Name = originalTour.Name,
                From = originalTour.From,
                To = originalTour.To,
                Distance = distance,
                Time = time,
                Description = originalTour.Description,
                TransportType = originalTour.TransportType,
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

        private async Task SaveTour(object? parameter)
        {
            if (SelectedTour != null)
            {
                try
                {
                    if (OriginalTour == null || string.IsNullOrEmpty(OriginalTour.Name))
                    {
                        if (!string.IsNullOrEmpty(SelectedTour.From) && !string.IsNullOrEmpty(SelectedTour.To))
                        {
                            try
                            {
                                var (distance, time) = await _routeDataManager.GetDistanceAndDurationAsync(SelectedTour.From, SelectedTour.To, SelectedTour.TransportType);
                                SelectedTour.Distance = distance;
                                SelectedTour.Time = time;
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"Error getting distance and time for new tour: {ex.Message}");
                                SelectedTour.Distance = null; // Set to null if distance couldn't be fetched
                                SelectedTour.Time = null; // Set to null if time couldn't be fetched
                            }
                        }
                        await _tourService.AddTourAsync(SelectedTour);
                        Tours.Add(SelectedTour);
                    }
                    else
                    {
                        OriginalTour.Name = SelectedTour.Name;
                        OriginalTour.From = SelectedTour.From;
                        OriginalTour.To = SelectedTour.To;
                        OriginalTour.Time = SelectedTour.Time;
                        OriginalTour.Description = SelectedTour.Description;
                        OriginalTour.TransportType = SelectedTour.TransportType;
                        if (!string.IsNullOrEmpty(OriginalTour.From) && !string.IsNullOrEmpty(OriginalTour.To))
                        {
                            try
                            {
                                var (distance, time) = await _routeDataManager.GetDistanceAndDurationAsync(OriginalTour.From, OriginalTour.To, OriginalTour.TransportType);
                                OriginalTour.Distance = distance;
                                OriginalTour.Time = time;
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"Error getting distance and time for existing tour: {ex.Message}");
                                OriginalTour.Distance = null;
                                OriginalTour.Time = null;
                            }
                        }
                        await _tourService.UpdateTourAsync(OriginalTour);
                    }

                    LoadTours();

                    if (parameter is Window window)
                    {
                        window.DialogResult = true;
                        window.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving tour: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("No tour selected to save.");
            }
        }

        private async Task DeleteSelectedTour(object? parameter)
        {
            if (SelectedTour != null && Tours != null)
            {
                var tourToDelete = Tours.FirstOrDefault(t => t.Id == SelectedTour.Id);
                if (tourToDelete != null)
                {
                    Tours.Remove(tourToDelete);
                    await _tourService.DeleteTourAsync(tourToDelete);
                }
                SelectedTour = null;
                OnPropertyChanged(nameof(Tours));
                OnPropertyChanged(nameof(SelectedTour));
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
