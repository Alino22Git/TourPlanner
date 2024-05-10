using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TourPlanner.ViewModels;
using TourPlannerBusinessLayer.Models;

namespace TourPlanner.Views
{
    public partial class MainWindow : Window
{
    private readonly MainViewModel mainViewModel;

    public MainWindow()
    {
        InitializeComponent();
        mainViewModel = new MainViewModel();
        DataContext = mainViewModel;
    }

    private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is ListBox listBox && listBox.SelectedItem is Tour selectedTour)
        {
            // Übergib die ursprüngliche Tour, um sie zu bearbeiten
            var addTourWindow = new AddTourWindow(mainViewModel.TourViewModel, selectedTour);
            bool? result = addTourWindow.ShowDialog();
            // Nur aktualisieren, wenn tatsächlich gespeichert wurde
            if (result == true)
            {
                mainViewModel.TourViewModel.OnPropertyChanged(nameof(mainViewModel.TourViewModel.Tours));
            }
        }
    }


        private void TourLogMenuItem_DoubleClick(object sender, MouseButtonEventArgs e)
    {
        Debug.WriteLine("TourLogMenuItem double-clicked!");

        if (sender is ListBox listBox && listBox.SelectedItem is TourLog selectedTourLog)
        {
            Debug.WriteLine($"Selected Tour Log ID: {selectedTourLog.Id}");
            var addTourLogWindow = new AddTourLogWindow(mainViewModel.TourLogViewModel);
            addTourLogWindow.ShowDialog();
        }
        else
        {
            Debug.WriteLine("TourLog not found in ListBox.");
        }
    }

    private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ListBox listBox && listBox.SelectedItem is Tour selectedTour)
        {
            mainViewModel.TourViewModel.SelectedTour = selectedTour;

            TourLogsListBox.ItemsSource = selectedTour.TourLogs;
            GeneralMenuItem_Click(sender, e);
        }
    }
         
    private void GeneralMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (mainViewModel.TourViewModel.SelectedTour != null)
        {
            DynamicContentControl.DataContext = mainViewModel.TourViewModel;
            DynamicContentControl.ContentTemplate = (DataTemplate)FindResource("TourDetailsTemplate");
            DynamicContentControl.Content = mainViewModel.TourViewModel.SelectedTour;
        }
    }

    private void RouteMenuItem_Click(object sender, RoutedEventArgs e)
    {
        DynamicContentControl.ContentTemplate = (DataTemplate)FindResource("RoutePlaceholderTemplate");
    }
}
}