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
    }
}