using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

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
                tours = value;
                OnPropertyChanged(nameof(Tours));
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

        public TourViewModel()
        {
            Tours = new ObservableCollection<Tour>()
            {
                new Tour { Name = "Tour 1", From = "Location 1", To = "Location 1", Distance = "10 km", Time = "00", Description = "Description 1" },
                new Tour { Name = "Tour 2", From = "Location 2", To = "Location 1", Distance = "15 km", Time = "00", Description = "Description 2" },
                new Tour { Name = "Tour 3", From = "Location 3", To = "Location 1", Distance = "20 km", Time = "00", Description = "Description 3" }
            };

            TourLogs = new ObservableCollection<TourLog>();
        }

        public void AddTour(Tour newTour)
        {
            Debug.Assert(Tours != null, nameof(Tours) + " != null");
            Tours.Add(newTour);
        }

        public void AddTourLog(TourLog newTourLog)
        {
            Debug.Assert(TourLogs != null, nameof(TourLogs) + " != null");
            TourLogs.Add(newTourLog);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
