using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;


namespace TourPlanner
{
    public partial class AddTourWindow : Window
    {
        private readonly TourViewModel viewModel;

        public AddTourWindow(TourViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Erstellen Sie eine neue Tour basierend auf den Eingaben im Fenster
            Tour newTour = new Tour
            {
                Name = NameTextBox.Text,
                From = FromTextBox.Text,
                To = FromTextBox.Text,
                Distance = DistanceTextBox.Text,
                Time = TimeTextBox.Text,
                Description = DescriptionTextBox.Text
            };

            // Fügen Sie die neue Tour zum ViewModel hinzu
            viewModel.AddTour(newTour);

            // Fenster schließen
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Fenster schließen
            Close();
        }
    }
}