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

        private Tour? OriginalTour { get; set; }

        public TourViewModel(TourService tourService)
        {
            _tourService = tourService;
            LoadTours();
            SaveTourCommand = new RelayCommand(async (parameter) => await SaveTour(parameter));
            DeleteTourCommand = new RelayCommand(DeleteSelectedTour);
        }

        private async void LoadTours()
        {
            var toursFromDb = await _tourService.GetToursAsync();
            Tours = new ObservableCollection<Tour>(toursFromDb);
            var exampleTourLogs = TourLog.CreateExampleTourLogs();
            Tours[0].TourLogs = new ObservableCollection<TourLog>(exampleTourLogs);
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

                // Speichern in die Datenbank
                await _tourService.AddTourAsync(OriginalTour);

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
                var tourToDelete = Tours.FirstOrDefault(t => t.Id == SelectedTour.Id);
                if (tourToDelete != null)
                {
                    Tours.Remove(tourToDelete);
                    // Löschen aus der Datenbank
                    // await _tourService.DeleteTourAsync(tourToDelete); // Implementieren Sie diese Methode im TourService
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
