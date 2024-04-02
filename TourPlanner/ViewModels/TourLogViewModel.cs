using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    public class TourLogViewModel
    {
        //===================================================================================================================================================================
        //TODO: auslagerung der gesamten logik aus dem AddTourLogWindow in dieses ViewModel, sodass das ViewModel die Logik enthält und das Fenster nur noch die View ist.
        //TODO: aufteilen des TourViewModels in TourLogViewModel und TourViewModel
        //===================================================================================================================================================================

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

        public TourLogViewModel()
        {
            
        }

        

        private ObservableCollection<TourLog> InitializeTourLogs()
        {

            TourLogs = new ObservableCollection<TourLog>(TourLog.CreateExampleTourLogs());
            return TourLogs;
        }

       

        public void AddTourLog(TourLog newTourLog)
        {
            Debug.Assert(TourLogs != null, nameof(TourLogs) + " != null");
            TourLogs.Add(newTourLog);
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
