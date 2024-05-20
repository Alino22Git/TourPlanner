using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Models;
using TourPlanner.Viewmodels;
using TourPlanner.Views;
using TourPlannerBusinessLayer.Services;

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

        public ICommand ListBoxItemDoubleClickCommand { get; }
        public ICommand TourLogMenuItemDoubleClickCommand { get; }
        public ICommand ListBoxSelectionChangedCommand { get; }

        private ContentControl _dynamicContentControl;
        public ContentControl DynamicContentControl
        {
            get => _dynamicContentControl;
            set
            {
                _dynamicContentControl = value;
                OnPropertyChanged(nameof(DynamicContentControl));
            }
        }

        public MainViewModel(ContentControl dynamicContentControl, TourService tourService, TourLogService tourLogService)
        {
            _dynamicContentControl = dynamicContentControl;

            TourViewModel = new TourViewModel(tourService);
            TourLogViewModel = new TourLogViewModel(TourViewModel, tourLogService);
            TourViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(TourViewModel.SelectedTour))
                {
                    TourLogViewModel.SelectedTour = TourViewModel.SelectedTour;
                    Debug.WriteLine($"SelectedTour changed: {TourViewModel.SelectedTour?.Name}");
                }
            };
            Debug.WriteLine($"SelectedTour changed: {TourViewModel.SelectedTour?.Name}");

            OpenAddTourWindowCommand = new RelayCommand(OpenAddTourWindow);
            OpenAddTourLogWindowCommand = new RelayCommand(OpenAddTourLogWindow);
            SetGeneralContentCommand = new RelayCommand(SetGeneralContent);
            SetRouteContentCommand = new RelayCommand(SetRouteContent);

            ListBoxItemDoubleClickCommand = new RelayCommand(ListBoxItemDoubleClick);
            TourLogMenuItemDoubleClickCommand = new RelayCommand(TourLogMenuItemDoubleClick);
            ListBoxSelectionChangedCommand = new RelayCommand(ListBoxSelectionChanged);
        }

        private void SetGeneralContent(object parameter)
        {
            if (TourViewModel.SelectedTour != null)
            {
                DynamicContentControl.DataContext = TourViewModel;
                DynamicContentControl.ContentTemplate = (DataTemplate)Application.Current.MainWindow.FindResource("TourDetailsTemplate");
                DynamicContentControl.Content = TourViewModel.SelectedTour;
            }
        }

        private void SetRouteContent(object parameter)
        {
            DynamicContentControl.ContentTemplate = (DataTemplate)Application.Current.MainWindow.FindResource("RoutePlaceholderTemplate");
        }

        private void OpenAddTourWindow(object? parameter)
        {
            TourViewModel.CreateNewTour(parameter);
        }

        private void OpenAddTourLogWindow(object? parameter)
        {
            var addTourLogWindow = new AddTourLogWindow(TourLogViewModel);
            addTourLogWindow.ShowDialog();
        }

        private void ListBoxItemDoubleClick(object parameter)
        {
            Debug.WriteLine("ListBoxItemDoubleClick called");
            if (parameter == null)
            {
                Debug.WriteLine("Parameter is null");
            }
            else
            {
                Debug.WriteLine($"Parameter type: {parameter.GetType()}");
            }

            if (parameter is Tour selectedTour)
            {
                Debug.WriteLine("Parameter is Tour");
                var addTourWindow = new AddTourWindow(TourViewModel, selectedTour);
                bool? result = addTourWindow.ShowDialog();
                if (result == true)
                {
                    TourViewModel.OnPropertyChanged(nameof(TourViewModel.Tours));
                }
            }
            else
            {
                Debug.WriteLine("Parameter is not Tour");
            }
        }

        private void TourLogMenuItemDoubleClick(object parameter)
        {
            if (parameter is TourLog selectedTourLog)
            {
                var addTourLogWindow = new AddTourLogWindow(TourLogViewModel);
                addTourLogWindow.ShowDialog();
            }
        }

        private void ListBoxSelectionChanged(object parameter)
        {
            if (parameter is Tour selectedTour)
            {
                TourViewModel.SelectedTour = selectedTour;
                TourLogViewModel.TourLogs = selectedTour.TourLogs;
                SetGeneralContent(selectedTour);
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
