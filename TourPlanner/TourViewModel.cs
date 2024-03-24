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

        public TourViewModel()
        {
            Tours = new ObservableCollection<Tour>();
        }

        public void AddTour(Tour newTour)
        {
            Debug.Assert(Tours != null, nameof(Tours) + " != null");
            Tours.Add(newTour);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
