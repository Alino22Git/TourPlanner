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


namespace TourPlanner
{
    public partial class MainWindow : Window
    {
        private readonly TourViewModel tourViewModel;

        public MainWindow()
        {
            InitializeComponent();

            // Erstellen Sie eine Instanz des TourViewModels
            tourViewModel = new TourViewModel();

            // Setzen Sie das DataContext des MainWindow auf das TourViewModel,
            // damit die Datenbindung funktioniert
            DataContext = tourViewModel;
        }

        private void AddTourMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Öffnen Sie das AddTourWindow und übergeben Sie das TourViewModel
            AddTourWindow addTourWindow = new AddTourWindow(tourViewModel);
            addTourWindow.ShowDialog();
        }

        private void AddTourLogMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Öffnen Sie das AddTourWindow und übergeben Sie das TourViewModel
            AddTourLogWindow addTourLogWindow = new AddTourLogWindow(tourViewModel);
            addTourLogWindow.ShowDialog();
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Überprüfen, ob das Ereignis ausgelöst wird
            Debug.WriteLine("ListBoxItem double-clicked!");

            // Überprüfen Sie den DataContext-Wert
            if (sender is ListBox listBox)
            {
                // Holen Sie die ID des ausgewählten Elements im ListBox
                if (listBox.SelectedValue is int tourId)
                {
                    // Holen Sie das ausgewählte Tour-Objekt aus dem ViewModel
                    Tour? selectedTour = tourViewModel.FindTourById(tourId);
                    if (selectedTour != null)
                    {
                        Debug.WriteLine($"Selected Tour: {selectedTour.Name}");
                        // Öffnen Sie das AddTourWindow mit den Details der ausgewählten Tour
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

            // Überprüfen Sie den DataContext-Wert
            if (sender is ListBox listBox)
            {
                // Holen Sie das ausgewählte TourLog-Objekt aus dem ListBox
                if (listBox.SelectedItem is TourLog selectedTourLog)
                {
                    Debug.WriteLine($"Selected Tour Log ID: {selectedTourLog.Id}");
                    // Öffnen Sie das AddTourLogWindow mit den Details des ausgewählten Tour-Logs
                    AddTourLogWindow addTourLogWindow = new AddTourLogWindow(tourViewModel, selectedTourLog);
                    addTourLogWindow.ShowDialog();
                }
                else
                {
                    Debug.WriteLine("TourLog not found in ListBox.");
                }
            }
        }

    }
}