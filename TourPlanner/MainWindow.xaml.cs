using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TourPlanner.Models;
using TourPlanner.ViewModels;


namespace TourPlanner
{
    public partial class MainWindow : Window
    {
        private readonly TourViewModel tourViewModel;

        public MainWindow()
        {
            InitializeComponent();

            
            tourViewModel = new TourViewModel();

          //Setzen des Data Contexts
            DataContext = tourViewModel;
        }

        private void AddTourMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Öffnen des AddTourWindows und übergeben des TourViewModels
            AddTourWindow addTourWindow = new AddTourWindow(tourViewModel);
            addTourWindow.ShowDialog();
        }

        private void AddTourLogMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Öffnen des AddTourLogWindows und übergeben des TourViewModels
            AddTourLogWindow addTourLogWindow = new AddTourLogWindow(tourViewModel);
            addTourLogWindow.ShowDialog();
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Überprüfen, ob das Ereignis ausgelöst wird
            Debug.WriteLine("ListBoxItem double-clicked!");

            // Überprüfen des DataContext-Werts
            if (sender is ListBox listBox)
            {
                // Holen der ID des ausgewählten Elements im ListBox
                if (listBox.SelectedValue is int tourId)
                {
                    // Holen des ausgewählten Tour-Objektes aus dem ViewModel
                    Tour? selectedTour = tourViewModel.FindTourById(tourId);
                    if (selectedTour != null)
                    {
                        Debug.WriteLine($"Selected Tour: {selectedTour.Name}");
                        // Öffnen  AddTourWindow mit den Details der ausgewählten Tour
                        AddTourWindow addTourWindow = new AddTourWindow(tourViewModel, selectedTour);
                        addTourWindow.ShowDialog();
                    }
                    else
                    {
                        Debug.WriteLine($"Tour with ID {tourId} not found.");
                    }
                }
            }
        }

        private void TourLogMenuItem_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Überprüfen, ob das Ereignis ausgelöst wird
            Debug.WriteLine("TourLogMenuItem double-clicked!");

            // Überprüfen des DataContext-Werts
            if (sender is ListBox listBox)
            {
                
                if (listBox.SelectedItem is TourLog selectedTourLog)
                {
                    Debug.WriteLine($"Selected Tour Log ID: {selectedTourLog.Id}");
                    AddTourLogWindow addTourLogWindow = new AddTourLogWindow(tourViewModel, selectedTourLog);
                    addTourLogWindow.ShowDialog();
                }
                else
                {
                    Debug.WriteLine("TourLog not found in ListBox.");
                }
            }
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem is Tour selectedTour)
            {
                tourViewModel.SelectedTour = selectedTour;
                TourLogsListBox.ItemsSource = selectedTour.TourLogs;
                GeneralMenuItem_Click(sender, e);
            }
        }
        private void GeneralMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (tourViewModel.SelectedTour != null)
            {
                DynamicContentControl.DataContext = tourViewModel;
                DynamicContentControl.ContentTemplate = (DataTemplate)FindResource("TourDetailsTemplate");
                DynamicContentControl.Content = tourViewModel.SelectedTour;
            }
        }

        private void RouteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DynamicContentControl.ContentTemplate = (DataTemplate)FindResource("RoutePlaceholderTemplate");
        }
    }
}