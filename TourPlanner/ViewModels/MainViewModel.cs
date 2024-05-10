using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TourPlannerBusinessLayer.Models;

namespace TourPlanner.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public TourViewModel TourViewModel { get; }
        public TourLogViewModel TourLogViewModel { get; }

        public ICommand OpenAddTourWindowCommand { get; }
        public ICommand OpenAddTourLogWindowCommand { get; }

        public ICommand SetGeneralContentCommand { get; }
        public ICommand SetRouteContentCommand { get; }

        public MainViewModel()
        {
            TourViewModel = new TourViewModel();
            TourLogViewModel = new TourLogViewModel(TourViewModel);
            TourViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(TourViewModel.SelectedTour))
                {
                    TourLogViewModel.SelectedTour = TourViewModel.SelectedTour;
                }
            };
            OpenAddTourWindowCommand = new RelayCommand(OpenAddTourWindow);
            OpenAddTourLogWindowCommand = new RelayCommand(OpenAddTourLogWindow);
            SetGeneralContentCommand = new RelayCommand(SetGeneralContent);
            SetRouteContentCommand = new RelayCommand(SetRouteContent);
        }


        


        private void SetGeneralContent(object parameter)
        {
            // Logik, um den Content zu setzen
        }

        private void SetRouteContent(object parameter)
        {
            // Logik, um den Content zu setzen
        }
        private void OpenAddTourWindow(object? parameter)
        {
            TourViewModel.CreateNewTour(parameter);
        }

        private void OpenAddTourLogWindow(object? parameter)
        {
            var addTourLogWindow = new Views.AddTourLogWindow(TourLogViewModel);
            addTourLogWindow.ShowDialog();
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}