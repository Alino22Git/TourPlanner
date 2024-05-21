using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Models;
using TourPlanner.Views;
using TourPlannerBusinessLayer.Services;

namespace TourPlanner.ViewModels
{
    public class TourViewModel : INotifyPropertyChanged
    {
        private readonly TourService _tourService;

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

        public ObservableCollection<string> TransportTypeOptions { get; } = new ObservableCollection<string>
        {
            "Hike",
            "Bicycle",
            "Car",
            "Motorcycle"
        };

        private Tour? OriginalTour { get; set; }

        public TourViewModel(TourService tourService)
        {
            _tourService = tourService;
            LoadTours();
            SaveTourCommand = new RelayCommand(async (parameter) => await SaveTour(parameter));
            DeleteTourCommand = new RelayCommand(async (parameter) => await DeleteSelectedTour(parameter));
        }

        private async void LoadTours()
        {
            var toursFromDb = await _tourService.GetToursAsync();
            Tours = new ObservableCollection<Tour>(toursFromDb);
        }

        public void OpenTourWindow(Tour originalTour)
        {
            StartEditing(originalTour);
            var addTourWindow = new AddTourWindow(this, originalTour);
            addTourWindow.ShowDialog();
        }

        public void StartEditing(Tour originalTour)
        {
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
                TransportType = originalTour.TransportType,
                TourLogs = new ObservableCollection<TourLog>(originalTour.TourLogs)
            };
            OnPropertyChanged(nameof(SelectedTour));
        }

        public void CreateNewTour(object? parameter)
        {
            // Ensure OriginalTour is null before opening the dialog
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
                    // Falls OriginalTour null ist, handelt es sich um eine neue Tour
                    if (OriginalTour == null)
                    {
                        await _tourService.AddTourAsync(SelectedTour);

                        // Tour zur UI-Liste hinzufügen
                        Tours.Add(SelectedTour);
                    }
                    else
                    {
                        // OriginalTour mit den Werten von SelectedTour aktualisieren
                        OriginalTour.Name = SelectedTour.Name;
                        OriginalTour.From = SelectedTour.From;
                        OriginalTour.To = SelectedTour.To;
                        OriginalTour.Distance = SelectedTour.Distance;
                        OriginalTour.Time = SelectedTour.Time;
                        OriginalTour.Description = SelectedTour.Description;
                        OriginalTour.TransportType = SelectedTour.TransportType;

                        // Tour in der Datenbank aktualisieren
                        await _tourService.UpdateTourAsync(OriginalTour);
                    }

                    // UI aktualisieren
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
