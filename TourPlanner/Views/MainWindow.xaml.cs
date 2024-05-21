using System;
using System.Windows;
using TourPlanner.ViewModels;

namespace TourPlanner.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();
            InitializeAsync();
            DataContext = mainViewModel;
        }

        private async void InitializeAsync()
        {
            await webView.EnsureCoreWebView2Async(null);
            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            // Navigiere zwei Verzeichnisse nach oben zum Projektverzeichnis
            string projectDir = System.IO.Path.GetFullPath(System.IO.Path.Combine(appDir, @"..\..\.."));
            string filePath = System.IO.Path.Combine(projectDir, "Resources/leaflet.html");
            webView.CoreWebView2.Navigate(filePath);
        }
    }
}