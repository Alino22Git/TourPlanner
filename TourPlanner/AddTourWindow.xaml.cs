using System.Windows;

namespace TourPlanner
{
    public partial class AddTourWindow : Window
    {
        private readonly TourViewModel viewModel;
        private Tour? selectedTour;

        // Konstruktor für das Hinzufügen einer neuen Tour
        public AddTourWindow(TourViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            selectedTour = new Tour(); // Neue Tour erstellen
            DataContext = selectedTour; // Datenkontext auf die neue Tour setzen
        }

        // Konstruktor für das Bearbeiten einer vorhandenen Tour
        public AddTourWindow(TourViewModel viewModel, Tour? selectedTour) : this(viewModel)
        {
            this.selectedTour = selectedTour;

            // Laden Sie die Daten der ausgewählten Tour in die Textfelder
            LoadSelectedTourData();
        }

        private void LoadSelectedTourData()
        {
            // Laden Sie die Daten der ausgewählten Tour in die Textfelder
            NameTextBox.Text = selectedTour?.Name;
            FromTextBox.Text = selectedTour?.From;
            ToTextBox.Text = selectedTour?.To;
            DistanceTextBox.Text = selectedTour?.Distance;
            TimeTextBox.Text = selectedTour?.Time;
            DescriptionTextBox.Text = selectedTour?.Description;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Aktualisieren Sie die Daten der ausgewählten Tour
            if (selectedTour != null)
            {
                selectedTour.Name = NameTextBox.Text;
                selectedTour.From = FromTextBox.Text;
                selectedTour.To = ToTextBox.Text;
                selectedTour.Distance = DistanceTextBox.Text;
                selectedTour.Time = TimeTextBox.Text;
                selectedTour.Description = DescriptionTextBox.Text;

                // Fügen Sie die Tour zum ViewModel hinzu oder aktualisieren Sie sie
                if (viewModel.Tours != null && !viewModel.Tours.Contains(selectedTour))
                    viewModel.AddTour(selectedTour);
                else
                    viewModel.UpdateTour(selectedTour);
            }

            // Fenster schließen
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Fenster schließen
            Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Überprüfen, ob ein Eintrag ausgewählt ist
            if (selectedTour != null)
            {
                // Entfernen Sie den ausgewählten Eintrag aus der Liste
                viewModel.Tours?.Remove(selectedTour);

                // Schließen Sie das Fenster
                Close();
            }
        }
    }
}
