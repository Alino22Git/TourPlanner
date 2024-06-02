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
using TourPlanner.Viewmodels;
using TourPlanner.Views;
using TourPlannerBusinessLayer.Managers;
using TourPlannerBusinessLayer.Service;
using TourPlannerLogging;
using TourPlanner.Exceptions;

namespace TourPlanner.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly RouteDataManager _routeDataManager;
        private readonly ReportManager _reportManager;
        private readonly TourService _tourService;
        private readonly DialogManager _dialogManager;
        private static readonly ILoggerWrapper _logger = LoggerFactory.GetLogger();

        public TourViewModel TourViewModel { get; }
        public TourLogViewModel TourLogViewModel { get; }

        public ICommand OpenAddTourWindowCommand { get; }
        public ICommand OpenAddTourLogWindowCommand { get; }
        public ICommand InitializeWebViewCommand { get; }
        public ICommand ListBoxItemDoubleClickCommand { get; }
        public ICommand TourLogMenuItemDoubleClickCommand { get; }
        public ICommand ListBoxSelectionChangedCommand { get; }
        public ICommand ReportGenCommand { get; }
        public ICommand SummaryReportGenCommand { get; }
        public ICommand GenerateReportWithMapScreenshotCommand { get; }
        public ICommand ExportToursCommand { get; }
        public ICommand ImportToursCommand { get; }

        private ContentControl _dynamicContentControl;
        private WebView2 _webView;
        private bool hideWebView = true;
        public ContentControl DynamicContentControl
        {
            get => _dynamicContentControl;
            set
            {
                _dynamicContentControl = value;
                OnPropertyChanged(nameof(DynamicContentControl));
            }
        }


        // Konstruktor initializing needed services and commands
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public MainViewModel(ContentControl dynamicContentControl, TourService tourService, TourLogService tourLogService, RouteDataManager routeDataManager, ReportManager reportManager, FileTransferManager fileTransferManager, DialogManager dialogManager)
        {
            _dynamicContentControl = dynamicContentControl;
            _routeDataManager = routeDataManager;
            _reportManager = reportManager;
            _tourService = tourService;
            _dialogManager = dialogManager;
            TourViewModel = new TourViewModel(tourService, routeDataManager);
            TourLogViewModel = new TourLogViewModel(TourViewModel, tourLogService, tourService);
            _logger.Debug("MainViewModel created");

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
            InitializeWebViewCommand = new RelayCommand(async (parameter) => await InitializeWebViewAsync(parameter as WebView2));
            ListBoxItemDoubleClickCommand = new RelayCommand(ListBoxItemDoubleClick);
            TourLogMenuItemDoubleClickCommand = new RelayCommand(TourLogMenuItemDoubleClick);
            ListBoxSelectionChangedCommand = new RelayCommand(ListBoxSelectionChanged);
            ReportGenCommand = new RelayCommand(GenerateReport);
            GenerateReportWithMapScreenshotCommand = new RelayCommand(GenerateReportWithMapScreenshot);
            SummaryReportGenCommand = new RelayCommand(GenerateSummaryReport);

            ExportToursCommand = new RelayCommand(async (parameter) =>
            {
                try
                {
                    var fileName = _dialogManager.ShowSaveFileDialog("JSON files (*.json)|*.json|All files (*.*)|*.*", "json", "export.json");
                    if (fileName != null)
                    {
                        await fileTransferManager.ExportToursAsync(fileName);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error($"Error exporting tours: {ex.Message}");
                    throw new SaveException("Error exporting tours", ex);
                }
            });

            ImportToursCommand = new RelayCommand(async (parameter) =>
            {
                try
                {
                    var fileName = _dialogManager.ShowOpenFileDialog("JSON files (*.json)|*.json|All files (*.*)|*.*", "json");
                    if (fileName != null)
                    {
                        await fileTransferManager.ImportToursAsync(fileName);
                        TourViewModel.LoadTours();
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error($"Error importing tours: {ex.Message}");
                    throw new SaveException("Error importing tours", ex);
                }
            });
        }

        //Initializes the WebView
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
        private async Task InitializeWebViewAsync(WebView2 webView)
        {
            try
            {
                if (webView != null)
                {
                    _webView = webView;
                    await webView.EnsureCoreWebView2Async(null);
                    await UpdateWebViewAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error initializing WebView: {ex.Message}");
                throw new InitializationException("Error initializing WebView", ex);
            }
        }

        //Updates the WebView
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private async Task UpdateWebViewAsync()
        {
            try
            {
                if (_webView != null && TourViewModel.SelectedTour != null && TourLogViewModel.SelectedTour.From != null)
                {
                    var (startLongitude, startLatitude, endLongitude, endLatitude, directions) = await _routeDataManager.GetTourDataAsync(TourViewModel.SelectedTour.From, TourViewModel.SelectedTour.To, TourViewModel.SelectedTour.TransportType);

                    string filePath = _routeDataManager.GetProjectResourcePath("directions.js");
                    await _routeDataManager.SaveDirectionsToFileAsync(directions, filePath);

                    string htmlPath = _routeDataManager.GetProjectResourcePath("leaflet.html");
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
            catch (Exception ex)
            {
                _logger.Error($"Error updating WebView: {ex.Message}");
                throw new UpdateException("Error updating WebView", ex);
            }
        }

        //Sets the general Content in the MainWindow
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetGeneralContentOnListChange(object parameter)
        {
            try
            {
                if (TourViewModel.SelectedTour != null)
                {
                    DynamicContentControl.DataContext = TourViewModel;
                    DynamicContentControl.ContentTemplate = (DataTemplate)Application.Current.MainWindow.FindResource("TourDetailsTemplate");
                    DynamicContentControl.Content = TourViewModel.SelectedTour;
                    if (_webView != null)
                    {
                        _webView.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error setting general content on list change: {ex.Message}");
                throw new UpdateException("Error setting general content on list change", ex);
            }
        }


        //Opens the AddTourWindow
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void OpenAddTourWindow(object? parameter)
        {
            try
            {
                TourViewModel.SelectedTour = null;
                _dialogManager.ShowAddTourWindow(TourViewModel);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error opening add tour window: {ex.Message}");
                throw new InitializationException("Error opening add tour window", ex);
            }
        }

        //Opens the AddTourLogWindow
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void OpenAddTourLogWindow(object? parameter)
        {
            try
            {
                _dialogManager.ShowAddTourLogWindow(TourLogViewModel);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error opening add tour log window: {ex.Message}");
                throw new InitializationException("Error opening add tour log window", ex);
            }
        }

        //Shows the EditTourWindow if there is no error
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void ListBoxItemDoubleClick(object parameter)
        {
            try
            {
                Debug.WriteLine("ListBoxItemDoubleClick called");
                if (parameter == null)
                {
                    _logger.Error("Parameter is null");
                    Debug.WriteLine("Parameter is null");
                }
                else
                {
                    Debug.WriteLine($"Parameter type: {parameter.GetType()}");
                }

                if (parameter is Tour selectedTour)
                {
                    Debug.WriteLine("Parameter is Tour");
                    _dialogManager.ShowEditTourWindow(TourViewModel, selectedTour);
                }
                else
                {
                    Debug.WriteLine("Parameter is not Tour");
                    _logger.Error("Parameter is not Tour");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error on list box item double click: {ex.Message}");
                throw new UpdateException("Error on list box item double click", ex);
            }
        }

        //Shows the EditTourLogWindow if there is no error
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void TourLogMenuItemDoubleClick(object parameter)
        {
            try
            {
                if (parameter is TourLog selectedTourLog)
                {
                    _dialogManager.ShowAddTourLogWindow(TourLogViewModel);
                }
                else
                {
                    _logger.Error("Parameter is not TourLog");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error on tour log menu item double click: {ex.Message}");
                throw new UpdateException("Error on tour log menu item double click", ex);
            }
        }

        //Sets the general content in the MainWindow when the ListBox selection changes
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void ListBoxSelectionChanged(object parameter)
        {
            try
            {
                if (parameter is Tour selectedTour)
                {
                    TourViewModel.SelectedTour = selectedTour;
                    TourLogViewModel.TourLogs = selectedTour.TourLogs;
                    SetGeneralContentOnListChange(selectedTour);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error on list box selection changed: {ex.Message}");
                throw new UpdateException("Error on list box selection changed", ex);
            }
        }

        //Sets up the report generation
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void GenerateReport(object parameter)
        {
            try
            {
                if (TourViewModel.SelectedTour != null)
                {
                    var fileName = _dialogManager.ShowSaveFileDialog("PDF files (*.pdf)|*.pdf|All files (*.*)|*.*", "pdf", $"{TourViewModel.SelectedTour.Name}.pdf");
                    if (fileName != null)
                    {
                        _reportManager.GenerateReport(TourViewModel.SelectedTour, fileName, _tourService);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a tour to generate a report.", "No tour selected", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error generating report: {ex.Message}");
                throw new SaveException("Error generating report", ex);
            }
        }

        //Sets up the report generation with a map screenshot
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void GenerateReportWithMapScreenshot(object parameter)
        {
            try
            {
                if (TourViewModel.SelectedTour != null)
                {
                    var fileName = _dialogManager.ShowSaveFileDialog("PDF files (*.pdf)|*.pdf|All files (*.*)|*.*", "pdf", $"{TourViewModel.SelectedTour.Name}.pdf");
                    if (fileName != null)
                    {
                        _reportManager.GenerateReportWithMapScreenshot(TourViewModel.SelectedTour, fileName, _tourService, _webView);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a tour to generate a report.", "No tour selected", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error generating report with map screenshot: {ex.Message}");
                throw new SaveException("Error generating report with map screenshot", ex);
            }
        }

        //Sets up the summary report generation
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void GenerateSummaryReport(object parameter)
        {
            try
            {
                var fileName = _dialogManager.ShowSaveFileDialog("PDF files (*.pdf)|*.pdf|All files (*.*)|*.*", "pdf", "SummaryReport.pdf");
                if (fileName != null)
                {
                    _reportManager.GenerateSummaryReport(fileName, _tourService);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error generating summary report: {ex.Message}");
                throw new SaveException("Error generating summary report", ex);
            }
        }

        //On Prperty Change method
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
