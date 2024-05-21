using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Web.WebView2.Wpf;
using Models;
using TourPlanner.Services;
using TourPlanner.Viewmodels;
using TourPlanner.Views;
using TourPlannerBusinessLayer.Services;

namespace TourPlanner.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly GeocodeService _geocodeService;
        private readonly DirectionService _directionService;

        public TourViewModel TourViewModel { get; }
        public TourLogViewModel TourLogViewModel { get; }

        public ICommand OpenAddTourWindowCommand { get; }
        public ICommand OpenAddTourLogWindowCommand { get; }
        public ICommand InitializeWebViewCommand { get; }

        public ICommand SetGeneralContentCommand { get; }
        public ICommand SetRouteContentCommand { get; }

        public ICommand ListBoxItemDoubleClickCommand { get; }
        public ICommand TourLogMenuItemDoubleClickCommand { get; }
        public ICommand ListBoxSelectionChangedCommand { get; }

        private ContentControl _dynamicContentControl;
        private WebView2 _webView;

        public ContentControl DynamicContentControl
        {
            get => _dynamicContentControl;
            set
            {
                _dynamicContentControl = value;
                OnPropertyChanged(nameof(DynamicContentControl));
            }
        }

        public MainViewModel(ContentControl dynamicContentControl, TourService tourService, TourLogService tourLogService, GeocodeService geocodeService, DirectionService directionService)
        {
            _dynamicContentControl = dynamicContentControl;

            TourViewModel = new TourViewModel(tourService);
            TourLogViewModel = new TourLogViewModel(TourViewModel, tourLogService);
            _geocodeService = geocodeService;
            _directionService = directionService;

            TourViewModel.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(TourViewModel.SelectedTour))
                {
                    TourLogViewModel.SelectedTour = TourViewModel.SelectedTour;
                    Debug.WriteLine($"SelectedTour changed: {TourViewModel.SelectedTour?.Name}");
                    await UpdateWebViewAsync();
                }
            };
            Debug.WriteLine($"SelectedTour changed: {TourViewModel.SelectedTour?.Name}");

            OpenAddTourWindowCommand = new RelayCommand(OpenAddTourWindow);
            OpenAddTourLogWindowCommand = new RelayCommand(OpenAddTourLogWindow);
            SetGeneralContentCommand = new RelayCommand(SetGeneralContent);
            SetRouteContentCommand = new RelayCommand(SetRouteContent);
            InitializeWebViewCommand = new RelayCommand(async (parameter) => await InitializeWebViewAsync(parameter as WebView2));
            ListBoxItemDoubleClickCommand = new RelayCommand(ListBoxItemDoubleClick);
            TourLogMenuItemDoubleClickCommand = new RelayCommand(TourLogMenuItemDoubleClick);
            ListBoxSelectionChangedCommand = new RelayCommand(ListBoxSelectionChanged);
        }

        private async Task InitializeWebViewAsync(WebView2 webView)
        {
            if (webView != null)
            {
                _webView = webView; // Speichern Sie die WebView-Referenz für spätere Updates
                await webView.EnsureCoreWebView2Async(null);
                await UpdateWebViewAsync();
            }
        }

        private async Task UpdateWebViewAsync()
        {
            if (_webView != null && TourViewModel.SelectedTour != null)
            {
                var (startLongitude, startLatitude) = await _geocodeService.GetCoordinatesAsync(TourViewModel.SelectedTour.From);
                var (endLongitude, endLatitude) = await _geocodeService.GetCoordinatesAsync(TourViewModel.SelectedTour.To);

                string directions = await _directionService.GetDirectionsAsync(startLongitude, startLatitude, endLongitude, endLatitude);

                string jsContent = $"const directions = {directions};";

                string projectDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
                string resourceDir = Path.Combine(projectDir, "Resources");
                Directory.CreateDirectory(resourceDir); // Sicherstellen, dass das Verzeichnis existiert
                string filePath = Path.Combine(resourceDir, "directions.js");
                await File.WriteAllTextAsync(filePath, jsContent);

                string htmlPath = Path.Combine(resourceDir, "leaflet.html");
                if (File.Exists(htmlPath))
                {
                    _webView.CoreWebView2.Navigate(htmlPath);
                }
                else
                {
                    MessageBox.Show($"Die Datei {htmlPath} wurde nicht gefunden.", "Datei nicht gefunden", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SetGeneralContent(object parameter)
        {
            if (TourViewModel.SelectedTour != null)
            {
                DynamicContentControl.DataContext = TourViewModel;
                DynamicContentControl.ContentTemplate = (DataTemplate)Application.Current.MainWindow.FindResource("TourDetailsTemplate");
                DynamicContentControl.Content = TourViewModel.SelectedTour;
            }

            if (_webView != null)
            {
                _webView.Visibility = Visibility.Collapsed;
            }
        }

        private void SetRouteContent(object parameter)
        
        {
            if (_webView != null)
            {
                _webView.Visibility = Visibility.Visible;
            }
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