using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
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
        }

        public void InitializeTours()
        {
            Tours = new ObservableCollection<Tour>()
            {
                new Tour { Id = 1, Name = "Tour 1", From = "Location 1", To = "Location 1", Distance = "10 km", Time = "2", Description = "Description 1", TourLogs = InitializeTourLogs()},
                new Tour { Id = 2, Name = "Tour 2", From = "Location 2", To = "Location 1", Distance = "15 km", Time = "12", Description = "Description 2" },
                new Tour { Id = 3, Name = "Tour 3", From = "Location 3", To = "Location 1", Distance = "20 km", Time = "4", Description = "Description 3" }
            };
        }

        private ObservableCollection<TourLog>  InitializeTourLogs()
        {
            
            TourLogs = new ObservableCollection<TourLog>(TourLog.CreateExampleTourLogs());
            return TourLogs;
        }

        public void AddTour(Tour newTour)
        {
            Debug.Assert(Tours != null, nameof(Tours) + " != null");
            Tours.Add(newTour);
            OnPropertyChanged(nameof(Tours)); 
        }

        public void AddTourLog(TourLog newTourLog)
        {
            Debug.Assert(TourLogs != null, nameof(TourLogs) + " != null");
            TourLogs.Add(newTourLog);
        }

        public void UpdateTour(Tour? updatedTour)
        {
            Debug.Assert(Tours != null, nameof(Tours) + " != null");

            Tour? existingTour = Tours.FirstOrDefault(tour => tour.Id == updatedTour?.Id);

            if (existingTour != null)
            {
                existingTour.Name = updatedTour?.Name;
                existingTour.From = updatedTour?.From;
                existingTour.To = updatedTour?.To;
                existingTour.Distance = updatedTour?.Distance;
                existingTour.Time = updatedTour?.Time;
                existingTour.Description = updatedTour?.Description;

                OnPropertyChanged(nameof(Tours));
            }
            else
            {
                Debug.WriteLine("Tour not found for update.");
            }
        }

        public void DeleteTourLog(TourLog selectedTour)
        {
            Debug.Assert(Tours != null, nameof(Tours) + " != null");

            if (selectedTour != null)
            {
                TourLogs.Remove(selectedTour);
                OnPropertyChanged(nameof(Tours));
            }
            else
            {
                Debug.WriteLine("Tour not found for delete.");
            }
        }

        public void UpdateTourLog(TourLog selectedTourLog)
        {
            Debug.Assert(TourLogs != null, nameof(TourLogs) + " != null");

            TourLog? existingTourLog = TourLogs.FirstOrDefault(log => log.Id == selectedTourLog.Id);

            if (existingTourLog != null)
            {
                existingTourLog.Date = selectedTourLog.Date;
                existingTourLog.Comment = selectedTourLog.Comment;
                existingTourLog.Difficulty = selectedTourLog.Difficulty;
                existingTourLog.TotalDistance = selectedTourLog.TotalDistance;
                existingTourLog.TotalTime = selectedTourLog.TotalTime;
                OnPropertyChanged(nameof(TourLogs));
            }
            else
            {
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
